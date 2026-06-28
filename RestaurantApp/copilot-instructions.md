GitHub Copilot Instructions — Restaurant SaaS
🧠 Project Context
This is a multi-tenant QR-based restaurant ordering SaaS built with:

Backend: ASP.NET Core Web API (.NET 8)
ORM: Entity Framework Core
Database: PostgreSQL
Auth: JWT Bearer Tokens
Password Hashing: BCrypt
Realtime: SignalR
Frontend: React + Vite

Customers scan a QR code on their table → browser opens → they view the menu and place orders → kitchen sees orders in real time.

🏗️ Solution Structure
Restaurant.sln
├── Restaurant.Domain          → Entities only. No logic.
├── Restaurant.Application     → Services, DTOs, Interface definitions
├── Restaurant.Infrastructure  → DbContext, EF Configurations, Repository implementations
└── Restaurant.API             → Controllers, Middleware, Program.cs
Layer Rules

Domain → entities and enums only. No EF, no business logic, no DTOs.
Application → all business logic lives here in services. Depends on Domain only.
Infrastructure → EF Core DbContext, entity configurations, migrations. Implements Application interfaces.
API → controllers call services only. Zero business logic in controllers.

👥 Roles
Role        Table         TenantId             
SuperAdmin  SuperAdmins   null — platformwide
Admin       Staffs        required
Kitchen     Staffs        required

SuperAdmin is stored in a separate table from Staff.
Staff always has a TenantId.
JWT token always carries role and tenantId (null for SuperAdmin).

🔒 CRITICAL — Multi-Tenancy Rules

These rules must never be broken. Every query must be tenant-scoped.

csharp// ✅ ALWAYS filter by TenantId
var items = await _db.MenuItems
    .Where(m => m.TenantId == tenantId && !m.IsDeleted)
    .ToListAsync();

// ❌ NEVER query without TenantId filter
var items = await _db.MenuItems.ToListAsync();

Extract tenantId from JWT claims in every controller/service — never trust input from request body for tenantId.
SuperAdmin endpoints skip TenantId filter — always guard with [Authorize(Roles = "SuperAdmin")].

🗑️ Soft Delete Pattern
All entities have IsDeleted and IsActive flags.
csharp// HasQueryFilter is set globally in DbContext for all entities
builder.HasQueryFilter(e => !e.IsDeleted);

// So in queries just write:
.Where(m => m.IsActive)
// IsDeleted filter is applied automatically via global query filter

Never hard delete any record — always set IsDeleted = true and UpdatedAt = DateTime.UtcNow.
For status toggling use IsActive. For removal use IsDeleted.

🔐 Authentication Rules
Login Flow
1. Check SuperAdmin table by email first
   → found + correct password → JWT with role = "SuperAdmin", tenantId = null
   → found + wrong password   → 401 "Invalid credentials" (STOP — do not check Staff)

2. Check Staff table by email
   → not found                → 401 "Invalid credentials"
   → IsActive = false         → 401 "Account is disabled"
   → LockedUntil > now        → 401 "Account locked. Try after X mins"
   → wrong password           → increment FailedLoginAttempts
                                 if >= 5 → set LockedUntil = now + 15 mins
                                 → 401 "Invalid credentials"
   → correct password         → reset FailedLoginAttempts = 0, LockedUntil = null
                                 update LastLoginAt → issue JWT

Password Rules
csharp// ✅ Always hash with BCrypt
staff.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

// ✅ Always verify with BCrypt
bool valid = BCrypt.Net.BCrypt.Verify(plainPassword, staff.PasswordHash);

// ❌ Never store plain text passwords
// ❌ Never use MD5 or SHA for passwords


JWT Claims
csharp// Always include these claims
new Claim("sub", user.Id.ToString()),
new Claim("role", roleName),
new Claim("tenantId", tenantId?.ToString() ?? ""),
new Claim("name", user.FirstName)
Security Rule

Always return "Invalid credentials" for both wrong email AND wrong password.
Never reveal which one is wrong — prevents enumeration attacks.

📱 QR Code Rules
csharp// ✅ Generate QrToken in service layer — never in entity constructor
var qrToken = Guid.NewGuid().ToString("N"); // 32 char, no dashes

// ✅ Build QrUrl from config — never hardcode domain
var qrUrl = $"{_config["AppBaseUrl"]}/menu/{qrToken}";

// ✅ Store all three fields
table.QrToken = qrToken;
table.QrUrl = qrUrl;
table.QrCodeImageUrl = await _qrService.GenerateAndUpload(qrUrl);
QrToken → unique identifier stored in DB, used to look up table
QrUrl → the URL encoded inside the QR image (https://yourapp.com/menu/{qrToken})
QrCodeImageUrl → the QR image file URL stored in CDN/S3 (admin downloads and prints this)


Public Menu Endpoint (no auth required)
GET /api/menu/{qrToken}
→ find DiningTable by QrToken
→ create TableSession
→ return menu + sessionToken

📦 Order Rules
OrderNumber Generation
csharp// Pattern: ORD-{tenantSlug}-{yyyyMMdd}-{sequence}
// Example: ORD-PIZZAHUT-20240427-001
// Always generate in service layer — never leave as empty string
Price Snapshot
csharp// ✅ Always copy price at time of order — never reference live price
orderItem.Price = menuItem.Price;          // snapshot
orderItem.TotalPrice = menuItem.Price * quantity;

// ❌ Never do this
orderItem.MenuItemId = id; // and then join to get price later for billing
Order Status Flow
Pending → Confirmed → Preparing → Ready → Served → Closed
                                                  ↑
                                   Cancelled (allowed from any stage)
Status Change Rule
csharp// Every status change MUST write to OrderStatusHistory
var history = new OrderStatusHistory
{
    OrderId = order.Id,
    TenantId = order.TenantId,
    FromStatus = order.Status,
    ToStatus = newStatus,
    ChangedByStaffId = staffId, // null if customer/system
    ChangedAt = DateTime.UtcNow
};
_db.OrderStatusHistories.Add(history);
Session Token
csharp// Customer places order using sessionToken (not tableId directly)
// Backend resolves tableId + tenantId from sessionToken
var session = await _db.TableSessions
    .FirstOrDefaultAsync(s => s.SessionToken == sessionToken && s.IsActive);

📝 Naming Conventions
DTOs
Create → CreateTableDto, CreateMenuItemDto, CreateOrderDto
Update → UpdateTableDto, UpdateMenuItemDto
Response → TableResponseDto, MenuItemResponseDto, OrderResponseDto
List → TableListDto, OrderListDto
Services
TableService, MenuItemService, OrderService, AuthService, QrService
Controllers
AuthController, TableController, MenuController, OrderController, TenantController
EF Configurations
DiningTableConfiguration, MenuItemConfiguration, OrderConfiguration

🔗 API Route Conventions
// SuperAdmin routes
/api/super/tenants
/api/super/plans

// Admin routes (tenant-scoped via JWT)
/api/admin/tables
/api/admin/menu
/api/admin/staff
/api/admin/orders

// Kitchen routes (tenant-scoped via JWT)
/api/kitchen/orders

// Public routes (no auth)
/api/menu/{qrToken}
/api/orders          ← customer places order using sessionToken
/api/auth/login

✅ Service Layer Pattern
csharp// Always follow this pattern in services
public async Task<ApiResponse<TableResponseDto>> CreateTableAsync(int tenantId, CreateTableDto dto)
{
    // 1. Validate input
    // 2. Check business rules (e.g. max tables per plan)
    // 3. Build entity
    // 4. Save to DB
    // 5. Return mapped DTO — never return raw entity
}

🚫 Never Do These

❌ Never put business logic in controllers
❌ Never return raw Entity objects from API — always map to DTOs
❌ Never store plain text passwords
❌ Never query without TenantId filter (except SuperAdmin)
❌ Never expose integer IDs in public-facing URLs — use QrToken or TenantGuid
❌ Never hard delete records — always soft delete (IsDeleted = true)
❌ Never generate QrToken or OrderNumber in entity constructors — do it in service layer
❌ Never trust tenantId from request body — always read from JWT claims
❌ Never reference live MenuItem.Price for billing — always use snapshot in OrderItem.Price
❌ Never change order status without writing to OrderStatusHistory

## 📤 API Response Pattern

Always use ApiResponse<T> from Restaurant.Application.Common for all service and controller responses.

// Service layer always returns ApiResponse<T>
public async Task<ApiResponse<MenuItemResponseDto>> GetMenuItemAsync(int id, int tenantId)

// Controller always uses StatusCode from the result
return StatusCode(result.StatusCode, result);

// Never return raw data directly from controller
// Never use IActionResult Ok(), NotFound(), BadRequest() directly
// Always go through ApiResponse<T> static methods


## Solif Principles
// Application should follow SOLID principles

using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Application.Common;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Authentication.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;
    private readonly IJwtService _jwtService;

    public RegistrationService(IRegistrationRepository repository, IJwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<object>> RegisterTenantAsync(
        string tenantName,
        string slug,
        string primaryEmail,
        string password,
        string? primaryPhone = null,
        int? countryId = null)
    {
        try
        {
            // Step 1: Validation
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(tenantName))
                validationErrors.Add("Tenant name is required");

            if (string.IsNullOrWhiteSpace(slug))
                validationErrors.Add("Slug is required");

            if (string.IsNullOrWhiteSpace(primaryEmail))
                validationErrors.Add("Email is required");

            if (!IsValidEmail(primaryEmail))
                validationErrors.Add("Invalid email format");

            if (string.IsNullOrWhiteSpace(password))
                validationErrors.Add("Password is required");

            if (password != null && password.Length < 8)
                validationErrors.Add("Password must be at least 8 characters long");

            if (validationErrors.Any())
            {
                return ApiResponse<object>.ValidationErrorResponse(
                    "Validation failed",
                    validationErrors);
            }

            // Step 2: Check if tenant exists by email
            var emailExists = await _repository.TenantExistsByEmailAsync(primaryEmail);

            // Step 3: If tenant exists, return bad request
            if (emailExists)
            {
                return ApiResponse<object>.ConflictResponse(
                    "Tenant registration failed",
                    new List<string> { "A tenant with this email already exists" });
            }

            // Step 4: Check if tenant exists by slug
            var slugExists = await _repository.TenantExistsBySlugAsync(slug);

            if (slugExists)
            {
                return ApiResponse<object>.ConflictResponse(
                    "Tenant registration failed",
                    new List<string> { "A tenant with this slug already exists" });
            }

            // Step 5: Add tenant
            var tenant = new Tenant
            {
                Name = tenantName,
                Slug = slug.ToLower(),
                PrimaryEmail = primaryEmail.ToLower(),
                PrimaryPhone = primaryPhone,
                CountryId = countryId,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdTenant = await _repository.CreateTenantAsync(tenant);

            // Step 6: Add default admin user
            var adminRole = await _repository.GetDefaultAdminRoleAsync();

            if (adminRole == null)
            {
                return ApiResponse<object>.ServerErrorResponse(
                    "Failed to create admin user: Default admin role not found");
            }

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var adminStaff = new Staff
            {
                TenantId = createdTenant.Id,
                Email = primaryEmail.ToLower(),
                PasswordHash = passwordHash,
                FirstName = tenantName,
                LastName = null,
                RoleId = adminRole.Id,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdStaff = await _repository.CreateStaffAsync(adminStaff);

            // Generate JWT token
            var token = _jwtService.GenerateToken(
                createdStaff.Id,
                createdStaff.Email,
                createdStaff.FirstName,
                adminRole.RoleName,
                createdTenant.Id);

            var response = new
            {
                Token = token,
                TenantId = createdTenant.Id,
                TenantName = createdTenant.Name,
                Email = createdStaff.Email,
                Role = adminRole.RoleName
            };

            return ApiResponse<object>.CreatedResponse(
                response,
                "Tenant registered successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<object>.ServerErrorResponse(
                "An error occurred during tenant registration. Please try again later.");
        }
    }


    public Task<ApiResponse<object>> RegisterStaffAsync(
        string email,
        string password,
        string firstName,
        string? lastName,
        int tenantId,
        int roleId)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<object>> ValidateEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

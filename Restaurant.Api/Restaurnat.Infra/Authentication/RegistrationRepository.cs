using Microsoft.Extensions.Logging;
using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Application.Common;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Authentication.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;
    private readonly IJwtService _jwtService;
    private readonly ILogger<RegistrationService> _logger;

    public RegistrationService(
        IRegistrationRepository repository, 
        IJwtService jwtService,
        ILogger<RegistrationService> _logger)
    {
        _repository = repository;
        _jwtService = jwtService;
        _logger = logger;
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
            _logger.LogInformation("Starting tenant registration for email: {Email}", primaryEmail);

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
                _logger.LogWarning("Validation failed for tenant registration: {Email}", primaryEmail);
                return ApiResponse<object>.ValidationErrorResponse(
                    "Validation failed",
                    validationErrors);
            }

            // Step 2: Check if tenant exists by email
            var emailExists = await _repository.TenantExistsByEmailAsync(primaryEmail);

            // Step 3: If tenant exists, return bad request
            if (emailExists)
            {
                _logger.LogWarning("Tenant registration failed: Email already exists - {Email}", primaryEmail);
                return ApiResponse<object>.ConflictResponse(
                    "Tenant registration failed",
                    new List<string> { "A tenant with this email already exists" });
            }

            // Step 4: Check if tenant exists by slug
            var slugExists = await _repository.TenantExistsBySlugAsync(slug);

            if (slugExists)
            {
                _logger.LogWarning("Tenant registration failed: Slug already exists - {Slug}", slug);
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
            _logger.LogInformation("Tenant created successfully with ID: {TenantId}", createdTenant.Id);

            // Step 6: Add default admin user
            var adminRole = await _repository.GetDefaultAdminRoleAsync();

            if (adminRole == null)
            {
                _logger.LogError("Default admin role not found in database");
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
            _logger.LogInformation("Admin staff created successfully with ID: {StaffId}", createdStaff.Id);

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

            _logger.LogInformation("Tenant registration completed successfully for: {Email}", primaryEmail);
            return ApiResponse<object>.CreatedResponse(
                response,
                "Tenant registered successfully");
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            // Database constraint violations, duplicate keys, foreign key violations
            _logger.LogError(ex, "Database update error during tenant registration for email: {Email}", primaryEmail);
            return ApiResponse<object>.ServerErrorResponse(
                "Failed to register tenant due to database error. Please try again later.");
        }
        catch (System.Data.Common.DbException ex)
        {
            // Database connection issues, timeout, network problems
            _logger.LogError(ex, "Database connection error during tenant registration for email: {Email}", primaryEmail);
            return ApiResponse<object>.ServerErrorResponse(
                "Database connection failed. Please try again later.");
        }
        catch (Exception ex)
        {
            // Any other unexpected errors
            _logger.LogCritical(ex, "Unexpected error during tenant registration for email: {Email}", primaryEmail);
            return ApiResponse<object>.ServerErrorResponse(
                "An unexpected error occurred during registration. Please try again later.");
        }
    }

    // Other methods...
}
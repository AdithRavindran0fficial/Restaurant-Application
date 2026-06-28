using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.Interfaces;

namespace Restaurant.Application.SuperAdmin.Services;

public class SuperAdminService : ISuperAdminService
{
    private readonly ISuperAdminRepository _repository;

    public SuperAdminService(ISuperAdminRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<object>> ChangePasswordAsync(
        int superAdminId,
        string currentPassword,
        string newPassword,
        string confirmPassword)
    {
        try
        {
            // Step 1: Validation
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(currentPassword))
                validationErrors.Add("Current password is required");

            if (string.IsNullOrWhiteSpace(newPassword))
                validationErrors.Add("New password is required");

            if (string.IsNullOrWhiteSpace(confirmPassword))
                validationErrors.Add("Confirm password is required");

            if (newPassword != null && newPassword.Length < 8)
                validationErrors.Add("New password must be at least 8 characters long");

            if (!string.IsNullOrWhiteSpace(newPassword) && 
                !string.IsNullOrWhiteSpace(confirmPassword) && 
                newPassword != confirmPassword)
                validationErrors.Add("New password and confirm password do not match");

            if (validationErrors.Any())
            {
                return ApiResponse<object>.ValidationErrorResponse(
                    "Validation failed",
                    validationErrors);
            }

            // Step 2: Get SuperAdmin by ID
            var superAdmin = await _repository.GetSuperAdminByIdAsync(superAdminId);

            if (superAdmin == null)
            {
                return ApiResponse<object>.NotFoundResponse("SuperAdmin not found");
            }

            // Step 3: Verify current password
            bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, superAdmin.PasswordHash);

            if (!isCurrentPasswordValid)
            {
                return ApiResponse<object>.UnauthorizedResponse("Current password is incorrect");
            }

            // Step 4: Check if new password is same as current password
            bool isSameAsOldPassword = BCrypt.Net.BCrypt.Verify(newPassword, superAdmin.PasswordHash);

            if (isSameAsOldPassword)
            {
                return ApiResponse<object>.ValidationErrorResponse(
                    "Password change failed",
                    new List<string> { "New password cannot be the same as current password" });
            }

            // Step 5: Hash new password and update
            superAdmin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _repository.UpdateSuperAdminAsync(superAdmin);

            return ApiResponse<object>.SuccessResponse(
                null,
                "Password changed successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<object>.ServerErrorResponse(
                "An error occurred while changing the password. Please try again later.");
        }
    }
}

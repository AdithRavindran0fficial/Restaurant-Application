using Restaurant.Application.Common;

namespace Restaurant.Application.SuperAdmin.Interfaces;

public interface ISuperAdminService
{
    Task<ApiResponse<object>> ChangePasswordAsync(
        int superAdminId,
        string currentPassword,
        string newPassword,
        string confirmPassword);
}

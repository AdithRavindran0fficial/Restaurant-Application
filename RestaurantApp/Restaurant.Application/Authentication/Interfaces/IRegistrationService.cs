using Restaurant.Application.Common;

namespace Restaurant.Application.Authentication.Interfaces;

public interface IRegistrationService
{
    Task<ApiResponse<object>> RegisterTenantAsync(
        string tenantName,
        string slug,
        string primaryEmail,
        string password,
        string? primaryPhone = null,
        int? countryId = null);

    Task<ApiResponse<object>> RegisterStaffAsync(string email, string password, string firstName, string? lastName, int tenantId, int roleId);
    Task<ApiResponse<object>> ValidateEmailAsync(string email);
}

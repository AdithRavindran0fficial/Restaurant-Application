using Restaurant.Domain.Entities;

namespace Restaurant.Application.Authentication.Interfaces;

public interface IRegistrationRepository
{
    // Tenant methods
    Task<bool> TenantExistsByEmailAsync(string email);
    Task<bool> TenantExistsBySlugAsync(string slug);
    Task<Tenant> CreateTenantAsync(Tenant tenant);

    // Staff methods
    Task<bool> EmailExistsInStaffAsync(string email);
    Task<Staff> CreateStaffAsync(Staff staff);

    // Role methods
    Task<Role?> GetRoleByIdAsync(int roleId);
    Task<Role?> GetDefaultAdminRoleAsync();

    // Tenant methods
    Task<Tenant?> GetTenantByIdAsync(int tenantId);
}

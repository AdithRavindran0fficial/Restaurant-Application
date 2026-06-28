using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.DeactivateTenant;

public interface IDeactivateTenantRepository
{
    Task<Tenant?> GetTenantByIdAsync(int tenantId);
    Task<bool> DeactivateTenantAsync(Tenant tenant);
}

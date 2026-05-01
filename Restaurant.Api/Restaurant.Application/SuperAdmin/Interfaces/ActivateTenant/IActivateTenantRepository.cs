using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.ActivateTenant;

public interface IActivateTenantRepository
{
    Task<Tenant?> GetTenantByIdAsync(int tenantId);
    Task<bool> ActivateTenantAsync(Tenant tenant);
}

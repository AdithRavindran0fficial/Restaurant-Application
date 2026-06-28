using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.GetAllTenants;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetAllTenantsAsync();
    Task<Tenant?> GetTenantByIdAsync(int tenantId);
}

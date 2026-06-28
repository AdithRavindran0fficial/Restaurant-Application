using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.SoftDeleteTenant;

public interface ISoftDeleteTenantRepository
{
    Task<Tenant?> GetTenantByIdAsync(int tenantId);
    Task<bool> SoftDeleteTenantAsync(Tenant tenant);
}

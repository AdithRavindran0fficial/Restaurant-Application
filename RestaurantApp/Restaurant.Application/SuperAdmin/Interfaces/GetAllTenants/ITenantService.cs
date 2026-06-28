using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces.GetAllTenants;

public interface ITenantService
{
    Task<ApiResponse<IEnumerable<TenantDto>>> GetAllTenantsAsync();
    Task<ApiResponse<TenantDto>> GetTenantByIdAsync(int tenantId);
}
    
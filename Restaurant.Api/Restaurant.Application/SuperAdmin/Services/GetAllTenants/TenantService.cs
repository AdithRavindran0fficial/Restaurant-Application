using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.GetAllTenants;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Services.GetAllTenants;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<ApiResponse<IEnumerable<TenantDto>>> GetAllTenantsAsync()
    {
        try
        {
            var tenants = await _tenantRepository.GetAllTenantsAsync();

            var tenantDtos = tenants.Select(t => new TenantDto
            {
                Id = t.Id,
                TenantGuid = t.TenantGuid,
                Name = t.Name,
                Slug = t.Slug,
                PrimaryEmail = t.PrimaryEmail,
                PrimaryPhone = t.PrimaryPhone,
                CountryId = t.CountryId,
                IsActive = t.IsActive,
                TrialEndsAt = t.TrialEndsAt,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList();

            if (!tenantDtos.Any())
            {
                return ApiResponse<IEnumerable<TenantDto>>.SuccessResponse(
                    tenantDtos,
                    "No tenants found");
            }

            return ApiResponse<IEnumerable<TenantDto>>.SuccessResponse(
                tenantDtos,
                "Tenants retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TenantDto>>.ServerErrorResponse(
                "An error occurred while retrieving tenants. Please try again later.");
        }
    }

    public async Task<ApiResponse<TenantDto>> GetTenantByIdAsync(int tenantId)
    {
        try
        {
            // Validation
            if (tenantId <= 0)
            {
                return ApiResponse<TenantDto>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            var tenant = await _tenantRepository.GetTenantByIdAsync(tenantId);

            if (tenant == null)
            {
                return ApiResponse<TenantDto>.NotFoundResponse(
                    $"Tenant with ID {tenantId} not found");
            }

            var tenantDto = new TenantDto
            {
                Id = tenant.Id,
                TenantGuid = tenant.TenantGuid,
                Name = tenant.Name,
                Slug = tenant.Slug,
                PrimaryEmail = tenant.PrimaryEmail,
                PrimaryPhone = tenant.PrimaryPhone,
                CountryId = tenant.CountryId,
                IsActive = tenant.IsActive,
                TrialEndsAt = tenant.TrialEndsAt,
                CreatedAt = tenant.CreatedAt,
                UpdatedAt = tenant.UpdatedAt
            };

            return ApiResponse<TenantDto>.SuccessResponse(
                tenantDto,
                "Tenant retrieved successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<TenantDto>.ServerErrorResponse(
                "An error occurred while retrieving the tenant. Please try again later.");
        }
    }
}

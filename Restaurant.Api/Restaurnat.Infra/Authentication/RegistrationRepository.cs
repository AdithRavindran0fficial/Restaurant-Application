using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.Authentication;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly MasterDbContext _context;

    public RegistrationRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailExistsInStaffAsync(string email)
    {
        return await _context.Staffs
            .AnyAsync(s => s.Email == email && !s.IsDeleted);
    }

    public async Task<Staff> CreateStaffAsync(Staff staff)
    {
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();
        return staff;
    }

    public async Task<Tenant?> GetTenantByIdAsync(int tenantId)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == tenantId && t.IsActive && !t.IsDeleted);
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId && !r.IsDeleted);
    }

    public async Task<bool> TenantExistsByEmailAsync(string email)
    {
        return await _context.Tenants
            .AnyAsync(t => t.PrimaryEmail == email && !t.IsDeleted);
    }

    public async Task<bool> TenantExistsBySlugAsync(string slug)
    {
        return await _context.Tenants
            .AnyAsync(t => t.Slug == slug && !t.IsDeleted);
    }

    public async Task<Tenant> CreateTenantAsync(Tenant tenant)
    {
        await _context.Tenants.AddAsync(tenant);
        await _context.SaveChangesAsync();
        return tenant;
    }

    public async Task<Role?> GetDefaultAdminRoleAsync()
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == "Admin" && r.IsSystem && !r.IsDeleted);
    }
}

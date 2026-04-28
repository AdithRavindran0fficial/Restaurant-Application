using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.Authentication;

public class AuthRepository : IAuthRepository
{
    private readonly MasterDbContext _context;

    public AuthRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<SuperAdmin?> GetSuperAdminByEmailAsync(string email)
    {
        return await _context.SuperAdmins
            .FirstOrDefaultAsync(sa => sa.Email == email && sa.IsActive);
    }

    public async Task<Staff?> GetStaffByEmailAsync(string email)
    {
        return await _context.Staffs
            .Include(s => s.Role)
            .Include(s => s.Tenant)
            .FirstOrDefaultAsync(s => s.Email == email && !s.IsDeleted);
    }

    public async Task UpdateStaffAsync(Staff staff)
    {
        _context.Staffs.Update(staff);
        await _context.SaveChangesAsync();
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId && !r.IsDeleted);
    }
}

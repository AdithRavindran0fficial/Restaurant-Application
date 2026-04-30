using Microsoft.EntityFrameworkCore;
using Restaurant.Application.SuperAdmin.Interfaces;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;

namespace Restaurnat.Infra.SuperAdmin;

public class SuperAdminRepository : ISuperAdminRepository
{
    private readonly MasterDbContext _context;

    public SuperAdminRepository(MasterDbContext context)
    {
        _context = context;
    }

    public async Task<Restaurant.Domain.Entities.SuperAdmin?> GetSuperAdminByIdAsync(int superAdminId)
    {
        return await _context.SuperAdmins
            .FirstOrDefaultAsync(sa => sa.Id == superAdminId && sa.IsActive);
    }

    public async Task UpdateSuperAdminAsync(Restaurant.Domain.Entities.SuperAdmin superAdmin)
    {
        _context.SuperAdmins.Update(superAdmin);
        await _context.SaveChangesAsync();
    }
}

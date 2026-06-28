using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Staff.UpdateStaff;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;
using StaffEntity = Restaurant.Domain.Entities.Staff;
using RoleEntity = Restaurant.Domain.Entities.Role;

namespace Restaurnat.Infra.Admin.Staff.UpdateStaff
{
    public class UpdateStaffRepository : IUpdateStaffRepository
    {
        private readonly MasterDbContext _context;

        public UpdateStaffRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<StaffEntity?> GetStaffByIdAsync(int tenantId, int staffId)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(s => s.Id == staffId && s.TenantId == tenantId && !s.IsDeleted);
        }

        public async Task<RoleEntity?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == roleId && !r.IsDeleted);
        }

        public async Task<bool> StaffEmailExistsAsync(int tenantId, string email, int excludeStaffId)
        {
            return await _context.Staffs
                .AnyAsync(s => s.TenantId == tenantId && s.Email.ToLower() == email.ToLower() && s.Id != excludeStaffId && !s.IsDeleted);
        }

        public async Task<bool> UpdateStaffAsync(StaffEntity staff)
        {
            _context.Staffs.Update(staff);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Staff.SoftDeleteStaff;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;
using StaffEntity = Restaurant.Domain.Entities.Staff;

namespace Restaurnat.Infra.Admin.Staff.SoftDeleteStaff
{
    public class SoftDeleteStaffRepository : ISoftDeleteStaffRepository
    {
        private readonly MasterDbContext _context;

        public SoftDeleteStaffRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<StaffEntity?> GetStaffByIdAsync(int tenantId, int staffId)
        {
            return await _context.Staffs
                .FirstOrDefaultAsync(s => s.Id == staffId && s.TenantId == tenantId);
        }

        public async Task<bool> SoftDeleteStaffAsync(StaffEntity staff)
        {
            staff.IsDeleted = true;
            staff.UpdatedAt = System.DateTime.UtcNow;
            _context.Staffs.Update(staff);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}

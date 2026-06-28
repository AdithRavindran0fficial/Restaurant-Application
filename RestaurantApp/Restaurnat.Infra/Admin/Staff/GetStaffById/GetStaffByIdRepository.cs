using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Admin.Interfaces.Staff.GetStaffById;
using Restaurnat.Infra.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Staff.GetStaffById
{
    public class GetStaffByIdRepository : IGetStaffByIdRepository
    {
        private readonly MasterDbContext _context;

        public GetStaffByIdRepository(MasterDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant.Domain.Entities.Staff?> GetStaffByIdAsync(int tenantId, int staffId)
        {
            return await _context.Staffs
                .Include(s => s.Role)
                .FirstOrDefaultAsync(s => s.Id == staffId && s.TenantId == tenantId && !s.IsDeleted);
        }
    }
}

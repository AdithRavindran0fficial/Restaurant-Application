using Restaurant.Application.Admin.Interfaces.Staff.GetAllStaff;
using Restaurant.Domain.Entities;
using Restaurnat.Infra.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurnat.Infra.Admin.Staff.GetAllStaff
{
    public class GetAllStaffRepository : IGetAllStaffRepository
    {
        private readonly MasterDbContext _context;

        public GetAllStaffRepository(MasterDbContext context)
        {
            _context = context;
        }

        public Task<List<Restaurant.Domain.Entities.Staff>> GetAllStaffAsync(int tenantId)
        {
            // TODO: implement
            throw new System.NotImplementedException();
        }
    }
}

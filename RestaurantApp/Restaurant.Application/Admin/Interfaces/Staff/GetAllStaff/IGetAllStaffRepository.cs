using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Staff.GetAllStaff
{
    public interface IGetAllStaffRepository
    {
        Task<List<Restaurant.Domain.Entities.Staff>> GetAllStaffAsync(int tenantId);
    }
}
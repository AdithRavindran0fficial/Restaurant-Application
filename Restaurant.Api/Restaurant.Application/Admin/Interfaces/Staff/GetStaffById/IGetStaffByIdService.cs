using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Staff.GetStaffById
{
    public interface IGetStaffByIdService
    {
        Task<ApiResponse<StaffDto>> GetStaffByIdAsync(int tenantId, int staffId);
    }
}

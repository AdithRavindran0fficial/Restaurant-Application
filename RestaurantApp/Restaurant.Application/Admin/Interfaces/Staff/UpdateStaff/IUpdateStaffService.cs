using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Staff.UpdateStaff
{
    public interface IUpdateStaffService
    {
        Task<ApiResponse<StaffDto>> UpdateStaffAsync(int tenantId, int staffId, UpdateStaffDto dto);
    }
}

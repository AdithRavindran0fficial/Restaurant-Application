using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Staff.GetAllStaff
{
    public interface IGetAllStaffService
    {
        Task<ApiResponse<List<StaffDto>>> GetAllStaffAsync(int tenantId);
    }
}

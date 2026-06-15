using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Staff.GetAllStaff;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Staff.GetAllStaff
{
    public class GetAllStaffService : IGetAllStaffService
    {
        private readonly IGetAllStaffRepository _repository;

        public GetAllStaffService(IGetAllStaffRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<StaffDto>>> GetAllStaffAsync(int tenantId)
        {
            return ApiResponse<List<StaffDto>>.ServerErrorResponse("Not implemented yet");
        }
    }
}

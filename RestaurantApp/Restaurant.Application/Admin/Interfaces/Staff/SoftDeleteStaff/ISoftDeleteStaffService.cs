using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Staff.SoftDeleteStaff
{
    public interface ISoftDeleteStaffService
    {
        Task<ApiResponse<bool>> SoftDeleteStaffAsync(int tenantId, int staffId);
    }
}

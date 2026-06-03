using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.SoftDeleteTable
{
    public interface ISoftDeleteTableService
    {
        Task<ApiResponse<bool>> SoftDeleteTableAsync(int tenantId, int tableId);
    }
}

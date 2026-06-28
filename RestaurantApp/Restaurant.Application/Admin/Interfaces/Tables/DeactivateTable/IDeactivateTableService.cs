using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.DeactivateTable
{
    public interface IDeactivateTableService
    {
        Task<ApiResponse<bool>> DeactivateTableAsync(int tenantId, int tableId);
    }
}

using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.ActivateTable
{
    public interface IActivateTableService
    {
        Task<ApiResponse<bool>> ActivateTableAsync(int tenantId, int tableId);
    }
}

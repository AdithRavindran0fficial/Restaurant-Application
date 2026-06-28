using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.UpdateTable
{
    public interface IUpdateTableService
    {
        Task<ApiResponse<DiningTableDto>> UpdateTableAsync(int tenantId, int tableId, UpdateTableDto dto);
    }
}

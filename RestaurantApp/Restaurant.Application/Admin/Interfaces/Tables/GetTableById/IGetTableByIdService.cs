using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.GetTableById
{
    public interface IGetTableByIdService
    {
        Task<ApiResponse<DiningTableDto>> GetTableByIdAsync(int tenantId, int tableId);
    }
}

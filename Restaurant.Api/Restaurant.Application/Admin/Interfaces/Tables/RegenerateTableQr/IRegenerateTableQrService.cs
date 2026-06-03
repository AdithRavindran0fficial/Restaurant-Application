using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.RegenerateTableQr
{
    public interface IRegenerateTableQrService
    {
        Task<ApiResponse<DiningTableDto>> RegenerateTableQrAsync(int tenantId, int tableId);
    }
}

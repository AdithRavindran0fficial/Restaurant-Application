using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.GetAllTables
{
    public interface IGetAllTablesService
    {
        Task<ApiResponse<List<DiningTableDto>>> GetAllTablesAsync(int tenantId);
    }
}

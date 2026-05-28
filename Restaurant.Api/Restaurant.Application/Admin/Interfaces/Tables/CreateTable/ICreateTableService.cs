using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.CreateTable
{
    public interface ICreateTableService
    {
        Task<ApiResponse<DiningTableDto>> CreateTableAsync(int tenantId, CreateTableDto dto);
    }
}

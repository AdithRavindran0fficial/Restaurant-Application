using Restaurant.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.GetAllTables
{
    public interface IGetAllTablesRepository
    {
        Task<List<DiningTable>> GetAllTablesAsync(int tenantId);
    }
}

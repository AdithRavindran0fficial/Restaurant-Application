using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.UpdateTable
{
    public interface IUpdateTableRepository
    {
        Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId);
        Task<bool> TableNumberExistsAsync(int tenantId, int tableNumber, int excludeTableId);
        Task<bool> UpdateTableAsync(DiningTable table);
    }
}

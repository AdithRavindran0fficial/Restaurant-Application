using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.DeactivateTable
{
    public interface IDeactivateTableRepository
    {
        Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId);
        Task<bool> DeactivateTableAsync(DiningTable table);
    }
}

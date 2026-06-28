using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.SoftDeleteTable
{
    public interface ISoftDeleteTableRepository
    {
        Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId);
        Task<bool> SoftDeleteTableAsync(DiningTable table);
    }
}

using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.RegenerateTableQr
{
    public interface IRegenerateTableQrRepository
    {
        Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId);
        Task<bool> UpdateTableAsync(DiningTable table);
    }
}

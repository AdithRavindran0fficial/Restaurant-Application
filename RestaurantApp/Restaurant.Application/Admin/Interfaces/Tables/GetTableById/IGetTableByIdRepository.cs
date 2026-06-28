using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.GetTableById
{
    public interface IGetTableByIdRepository
    {
        Task<DiningTable?> GetTableByIdAsync(int tenantId, int tableId);
    }
}

using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Tables.CreateTable
{
    public interface ICreateTableRepository
    {
        Task<bool> TableNumberExistsAsync(int tenantId, int tableNumber);
        Task<DiningTable> CreateTableAsync(DiningTable table);
    }
}

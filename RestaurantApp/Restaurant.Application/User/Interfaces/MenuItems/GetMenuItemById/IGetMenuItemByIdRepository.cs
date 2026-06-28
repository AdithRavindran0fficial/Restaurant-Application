using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemById
{
    public interface IGetMenuItemByIdRepository
    {
        Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId);
        Task<DiningTable?> GetDiningTableByQrTokenAsync(string qrToken);
    }
}

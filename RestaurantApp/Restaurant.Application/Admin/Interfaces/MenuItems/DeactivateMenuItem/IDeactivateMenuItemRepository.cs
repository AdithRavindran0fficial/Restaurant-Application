using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.DeactivateMenuItem
{
    public interface IDeactivateMenuItemRepository
    {
        Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId);
        Task<bool> DeactivateMenuItemAsync(MenuItem menuItem);
    }
}

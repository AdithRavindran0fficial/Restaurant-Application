using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.ActivateMenuItem
{
    public interface IActivateMenuItemRepository
    {
        Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId);
        Task<bool> ActivateMenuItemAsync(MenuItem menuItem);
    }
}

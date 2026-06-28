using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.UpdateMenuItem
{
    public interface IUpdateMenuItemRepository
    {
        Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId);
        Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId);
        Task<bool> MenuItemExistsAsync(int tenantId, string name, int categoryId, int excludeMenuItemId);
        Task<bool> UpdateMenuItemAsync(MenuItem menuItem);
    }
}

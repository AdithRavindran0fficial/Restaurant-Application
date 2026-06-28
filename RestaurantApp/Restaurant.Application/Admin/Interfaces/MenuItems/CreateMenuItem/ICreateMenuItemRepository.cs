using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.CreateMenuItem
{
    public interface ICreateMenuItemRepository
    {
        Task<bool> MenuItemExistsAsync(int tenantId, string name, int categoryId);
        Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId);
        Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem);
    }
}

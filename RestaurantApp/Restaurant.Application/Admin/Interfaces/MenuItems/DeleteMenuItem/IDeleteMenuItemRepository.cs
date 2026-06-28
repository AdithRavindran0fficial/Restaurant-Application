using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.DeleteMenuItem
{
    public interface IDeleteMenuItemRepository
    {
        Task<MenuItem?> GetMenuItemByIdAsync(int tenantId, int menuItemId);
        Task<bool> SoftDeleteMenuItemAsync(MenuItem menuItem);
    }
}

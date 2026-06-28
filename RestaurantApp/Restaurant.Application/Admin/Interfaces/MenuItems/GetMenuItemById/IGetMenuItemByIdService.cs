using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.GetMenuItemById
{
    public interface IGetMenuItemByIdService
    {
        Task<ApiResponse<MenuItemDto>> GetMenuItemByIdAsync(int tenantId, int menuItemId);
    }
}

using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemById
{
    public interface IGetMenuItemByIdService
    {
        Task<ApiResponse<MenuItemDto>> GetMenuItemByIdAsync(string qrToken, int menuItemId);
    }
}

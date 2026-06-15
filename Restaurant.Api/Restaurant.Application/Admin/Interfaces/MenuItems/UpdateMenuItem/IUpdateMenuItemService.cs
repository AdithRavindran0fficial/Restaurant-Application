using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.UpdateMenuItem
{
    public interface IUpdateMenuItemService
    {
        Task<ApiResponse<MenuItemDto>> UpdateMenuItemAsync(int tenantId, int menuItemId, UpdateMenuItemDto dto);
    }
}

using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.CreateMenuItem
{
    public interface ICreateMenuItemService
    {
        Task<ApiResponse<MenuItemDto>> CreateMenuItemAsync(int tenantId, CreateMenuItemDto dto);
    }
}

using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.GetAllMenuItems
{
    public interface IGetAllMenuItemsService
    {
        Task<ApiResponse<List<MenuItemDto>>> GetAllMenuItemsAsync(int tenantId);
    }
}

using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemsByCategoryId
{
    public interface IGetMenuItemsByCategoryIdService
    {
        Task<ApiResponse<List<MenuItemDto>>> GetMenuItemsByCategoryIdAsync(string qrToken, int categoryId);
    }
}

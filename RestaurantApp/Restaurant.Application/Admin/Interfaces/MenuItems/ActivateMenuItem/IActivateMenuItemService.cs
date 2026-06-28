using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.ActivateMenuItem
{
    public interface IActivateMenuItemService
    {
        Task<ApiResponse<bool>> ActivateMenuItemAsync(int tenantId, int menuItemId);
    }
}

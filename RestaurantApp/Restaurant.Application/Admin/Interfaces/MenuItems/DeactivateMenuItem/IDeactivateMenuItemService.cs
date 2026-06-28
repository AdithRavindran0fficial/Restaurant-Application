using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.DeactivateMenuItem
{
    public interface IDeactivateMenuItemService
    {
        Task<ApiResponse<bool>> DeactivateMenuItemAsync(int tenantId, int menuItemId);
    }
}

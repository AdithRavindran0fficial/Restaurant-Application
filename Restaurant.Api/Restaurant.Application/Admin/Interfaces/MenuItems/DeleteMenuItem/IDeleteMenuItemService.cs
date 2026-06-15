using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.DeleteMenuItem
{
    public interface IDeleteMenuItemService
    {
        Task<ApiResponse<bool>> DeleteMenuItemAsync(int tenantId, int menuItemId);
    }
}

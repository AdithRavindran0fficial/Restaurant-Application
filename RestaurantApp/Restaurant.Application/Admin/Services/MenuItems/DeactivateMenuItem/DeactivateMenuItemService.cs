using Restaurant.Application.Admin.Interfaces.MenuItems.DeactivateMenuItem;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.MenuItems.DeactivateMenuItem
{
    public class DeactivateMenuItemService : IDeactivateMenuItemService
    {
        private readonly IDeactivateMenuItemRepository _repository;

        public DeactivateMenuItemService(IDeactivateMenuItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> DeactivateMenuItemAsync(int tenantId, int menuItemId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            if (menuItemId <= 0)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Invalid menu item ID",
                    new List<string> { "Menu item ID must be greater than 0" });
            }

            var menuItem = await _repository.GetMenuItemByIdAsync(tenantId, menuItemId);
            if (menuItem == null)
            {
                return ApiResponse<bool>.NotFoundResponse($"Menu item with ID {menuItemId} not found");
            }

            if (menuItem.IsDeleted)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Cannot deactivate deleted menu item",
                    new List<string> { $"Menu item with ID {menuItemId} is marked as deleted" });
            }

            if (!menuItem.IsActive)
            {
                return ApiResponse<bool>.ValidationErrorResponse(
                    "Menu item already inactive",
                    new List<string> { $"Menu item with ID {menuItemId} is already inactive" });
            }

            var result = await _repository.DeactivateMenuItemAsync(menuItem);
            if (!result)
            {
                return ApiResponse<bool>.ServerErrorResponse("Failed to deactivate menu item. Please try again later.");
            }

            return ApiResponse<bool>.SuccessResponse(true, "Menu item deactivated successfully");
        }
    }
}

using Restaurant.Application.Admin.Interfaces.MenuItems.DeleteMenuItem;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.MenuItems.DeleteMenuItem
{
    public class DeleteMenuItemService : IDeleteMenuItemService
    {
        private readonly IDeleteMenuItemRepository _repository;

        public DeleteMenuItemService(IDeleteMenuItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> DeleteMenuItemAsync(int tenantId, int menuItemId)
        {
            try
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

                var existing = await _repository.GetMenuItemByIdAsync(tenantId, menuItemId);
                if (existing == null)
                {
                    return ApiResponse<bool>.NotFoundResponse($"Menu item with ID {menuItemId} not found");
                }

                if (existing.IsDeleted)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Menu item already deleted",
                        new List<string> { $"Menu item with ID {menuItemId} is already marked as deleted" });
                }

                var result = await _repository.SoftDeleteMenuItemAsync(existing);
                if (!result)
                {
                    return ApiResponse<bool>.ServerErrorResponse("Failed to delete menu item. Please try again later.");
                }

                return ApiResponse<bool>.SuccessResponse(true, "Menu item deleted successfully");
            }
            catch
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "An error occurred while deleting the menu item. Please try again later.");
            }
        }
    }
}

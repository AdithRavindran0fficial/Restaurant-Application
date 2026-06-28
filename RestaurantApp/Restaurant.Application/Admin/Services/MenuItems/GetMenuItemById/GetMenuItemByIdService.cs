using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.MenuItems.GetMenuItemById;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.MenuItems.GetMenuItemById
{
    public class GetMenuItemByIdService : IGetMenuItemByIdService
    {
        private readonly IGetMenuItemByIdRepository _repository;

        public GetMenuItemByIdService(IGetMenuItemByIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<MenuItemDto>> GetMenuItemByIdAsync(int tenantId, int menuItemId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            if (menuItemId <= 0)
            {
                return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                    "Invalid menu item ID",
                    new List<string> { "Menu item ID must be greater than 0" });
            }

            var menuItem = await _repository.GetMenuItemByIdAsync(tenantId, menuItemId);
            if (menuItem == null)
            {
                return ApiResponse<MenuItemDto>.NotFoundResponse($"Menu item with ID {menuItemId} not found");
            }

            var dto = new MenuItemDto
            {
                Id = menuItem.Id,
                TenantId = menuItem.TenantId,
                CategoryId = menuItem.CategoryId,
                CategoryName = menuItem.Category?.Name,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                ImageUrl = menuItem.ImageUrl,
                IsVeg = menuItem.IsVeg,
                PreparationTime = menuItem.PreparationTime,
                DisplayOrder = menuItem.DisplayOrder,
                IsAvailable = menuItem.IsAvailable,
                IsActive = menuItem.IsActive,
                CreatedAt = menuItem.CreatedAt,
                UpdatedAt = menuItem.UpdatedAt
            };

            return ApiResponse<MenuItemDto>.SuccessResponse(dto, "Menu item retrieved successfully");
        }
    }
}

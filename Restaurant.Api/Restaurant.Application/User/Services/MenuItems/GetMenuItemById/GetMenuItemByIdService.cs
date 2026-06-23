using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemById;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.User.Services.MenuItems.GetMenuItemById
{
    public class GetMenuItemByIdService : IGetMenuItemByIdService
    {
        private readonly IGetMenuItemByIdRepository _repository;

        public GetMenuItemByIdService(IGetMenuItemByIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<MenuItemDto>> GetMenuItemByIdAsync(string qrToken, int menuItemId)
        {
            if (string.IsNullOrWhiteSpace(qrToken))
            {
                return ApiResponse<MenuItemDto>.ValidationErrorResponse("QR token is required");
            }

            if (menuItemId <= 0)
            {
                return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                    "Invalid menu item ID",
                    new List<string> { "Menu item ID must be greater than 0" });
            }

            var table = await _repository.GetDiningTableByQrTokenAsync(qrToken);
            if (table == null)
            {
                return ApiResponse<MenuItemDto>.NotFoundResponse("Table not found");
            }

            var menuItem = await _repository.GetMenuItemByIdAsync(table.TenantId, menuItemId);
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

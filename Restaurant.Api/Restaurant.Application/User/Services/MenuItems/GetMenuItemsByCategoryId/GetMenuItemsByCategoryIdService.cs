using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemsByCategoryId;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Application.User.Services.MenuItems.GetMenuItemsByCategoryId
{
    public class GetMenuItemsByCategoryIdService : IGetMenuItemsByCategoryIdService
    {
        private readonly IGetMenuItemsByCategoryIdRepository _repository;

        public GetMenuItemsByCategoryIdService(IGetMenuItemsByCategoryIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<MenuItemDto>>> GetMenuItemsByCategoryIdAsync(string qrToken, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(qrToken))
            {
                return ApiResponse<List<MenuItemDto>>.ValidationErrorResponse("QR token is required");
            }

            if (categoryId <= 0)
            {
                return ApiResponse<List<MenuItemDto>>.ValidationErrorResponse("Category ID must be greater than 0");
            }

            var table = await _repository.GetDiningTableByQrTokenAsync(qrToken);

            if (table == null)
            {
                return ApiResponse<List<MenuItemDto>>.NotFoundResponse("Table not found");
            }

            var menuItems = await _repository.GetMenuItemsByCategoryIdAsync(table.TenantId, categoryId);

            var menuItemDtos = menuItems.Select(m => new MenuItemDto
            {
                Id = m.Id,
                TenantId = m.TenantId,
                CategoryId = m.CategoryId,
                CategoryName = m.Category?.Name,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsVeg = m.IsVeg,
                PreparationTime = m.PreparationTime,
                DisplayOrder = m.DisplayOrder,
                IsAvailable = m.IsAvailable,
                IsActive = m.IsActive,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            }).ToList();

            return ApiResponse<List<MenuItemDto>>.SuccessResponse(menuItemDtos);
            }
    }
}

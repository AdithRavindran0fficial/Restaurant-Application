using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.MenuItems.GetMenuItemsByCategoryId;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.MenuItems.GetMenuItemsByCategoryId
{
    public class GetMenuItemsByCategoryIdService : IGetMenuItemsByCategoryIdService
    {
        private readonly IGetMenuItemsByCategoryIdRepository _repository;

        public GetMenuItemsByCategoryIdService(IGetMenuItemsByCategoryIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<MenuItemDto>>> GetMenuItemsByCategoryIdAsync(int tenantId, int categoryId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<List<MenuItemDto>>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            if (categoryId <= 0)
            {
                return ApiResponse<List<MenuItemDto>>.ValidationErrorResponse(
                    "Invalid category ID",
                    new List<string> { "Category ID must be greater than 0" });
            }

            var menuItems = await _repository.GetMenuItemsByCategoryIdAsync(tenantId, categoryId);

            var dtos = menuItems.Select(m => new MenuItemDto
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

            return ApiResponse<List<MenuItemDto>>.SuccessResponse(dtos, $"{dtos.Count} menu item(s) retrieved successfully");
        }
    }
}

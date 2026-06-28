using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.MenuItems.GetAllMenuItems;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.MenuItems.GetAllMenuItems
{
    public class GetAllMenuItemsService : IGetAllMenuItemsService
    {
        private readonly IGetAllMenuItemsRepository _repository;

        public GetAllMenuItemsService(IGetAllMenuItemsRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<MenuItemDto>>> GetAllMenuItemsAsync(int tenantId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<List<MenuItemDto>>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            var menuItems = await _repository.GetAllMenuItemsAsync(tenantId);

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

            return ApiResponse<List<MenuItemDto>>.SuccessResponse(
                dtos,
                $"{dtos.Count} menu item(s) retrieved successfully");
        }
    }
}

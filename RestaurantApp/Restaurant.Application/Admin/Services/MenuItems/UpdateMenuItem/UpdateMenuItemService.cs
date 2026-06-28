using Microsoft.Extensions.Configuration;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.MenuItems.UpdateMenuItem;
using Restaurant.Application.Common;
using Restaurant.Application.Common.ImageServices;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.MenuItems.UpdateMenuItem
{
    public class UpdateMenuItemService : IUpdateMenuItemService
    {
        private readonly IUpdateMenuItemRepository _repository;
        private readonly IImageUploaderService _imageUploaderService;
        private readonly IConfiguration _configuration;

        public UpdateMenuItemService(
            IUpdateMenuItemRepository repository,
            IImageUploaderService imageUploaderService,
            IConfiguration configuration)
        {
            _repository = repository;
            _imageUploaderService = imageUploaderService;
            _configuration = configuration;
        }

        public async Task<ApiResponse<MenuItemDto>> UpdateMenuItemAsync(int tenantId, int menuItemId, UpdateMenuItemDto dto)
        {
            try
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

                if (dto.CategoryId <= 0)
                {
                    return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                        "Invalid category ID",
                        new List<string> { "Category ID must be greater than 0" });
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                        "Validation failed",
                        new List<string> { "Name is required" });
                }

                if (dto.Price <= 0)
                {
                    return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                        "Validation failed",
                        new List<string> { "Price must be greater than 0" });
                }

                var existing = await _repository.GetMenuItemByIdAsync(tenantId, menuItemId);
                if (existing == null)
                {
                    return ApiResponse<MenuItemDto>.NotFoundResponse($"Menu item with ID {menuItemId} not found");
                }

                var category = await _repository.GetCategoryByIdAsync(tenantId, dto.CategoryId);
                if (category == null)
                {
                    return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                        "Invalid category",
                        new List<string> { $"Category with ID {dto.CategoryId} not found for this tenant" });
                }

                if (!string.Equals(existing.Name, dto.Name, StringComparison.OrdinalIgnoreCase) || existing.CategoryId != dto.CategoryId)
                {
                    var duplicate = await _repository.MenuItemExistsAsync(tenantId, dto.Name, dto.CategoryId, menuItemId);
                    if (duplicate)
                    {
                        return ApiResponse<MenuItemDto>.ConflictResponse(
                            $"Menu item '{dto.Name}' already exists for this category");
                    }
                }

                string? imageUrl = existing.ImageUrl;
                if (dto.Image != null && dto.Image.Length > 0)
                {
                    await using var ms = new MemoryStream();
                    await dto.Image.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();

                    var folderName = "menu-items";
                    var fileName = $"menuitem-{tenantId}-{Guid.NewGuid():N}{Path.GetExtension(dto.Image.FileName)}";
                    imageUrl = await _imageUploaderService.UploadImageAsync(
                        imageBytes,
                        fileName,
                        tenantId.ToString(),
                        folderName,
                        dto.Image.ContentType);
                }

                existing.CategoryId = dto.CategoryId;
                existing.Name = dto.Name;
                existing.Description = dto.Description;
                existing.Price = dto.Price;
                existing.ImageUrl = imageUrl;
                existing.IsVeg = dto.IsVeg;
                existing.PreparationTime = dto.PreparationTime;
                existing.DisplayOrder = dto.DisplayOrder;
                existing.IsAvailable = dto.IsAvailable;
                existing.IsActive = dto.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;

                var updated = await _repository.UpdateMenuItemAsync(existing);
                if (!updated)
                {
                    return ApiResponse<MenuItemDto>.ServerErrorResponse("Failed to update menu item. Please try again later.");
                }

                var result = new MenuItemDto
                {
                    Id = existing.Id,
                    TenantId = existing.TenantId,
                    CategoryId = existing.CategoryId,
                    CategoryName = category.Name,
                    Name = existing.Name,
                    Description = existing.Description,
                    Price = existing.Price,
                    ImageUrl = existing.ImageUrl,
                    IsVeg = existing.IsVeg,
                    PreparationTime = existing.PreparationTime,
                    DisplayOrder = existing.DisplayOrder,
                    IsAvailable = existing.IsAvailable,
                    IsActive = existing.IsActive,
                    CreatedAt = existing.CreatedAt,
                    UpdatedAt = existing.UpdatedAt
                };

                return ApiResponse<MenuItemDto>.SuccessResponse(result, "Menu item updated successfully");
            }
            catch
            {
                return ApiResponse<MenuItemDto>.ServerErrorResponse(
                    "An error occurred while updating the menu item. Please try again later.");
            }
        }
    }
}

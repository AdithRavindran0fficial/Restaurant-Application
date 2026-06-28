using Microsoft.Extensions.Configuration;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.MenuItems.CreateMenuItem;
using Restaurant.Application.Common;
using Restaurant.Application.Common.ImageServices;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.MenuItems.CreateMenuItem
{
    public class CreateMenuItemService : ICreateMenuItemService
    {
        private readonly ICreateMenuItemRepository _repository;
        private readonly IImageUploaderService _imageUploaderService;
        private readonly IConfiguration _configuration;

        public CreateMenuItemService(
            ICreateMenuItemRepository repository,
            IImageUploaderService imageUploaderService,
            IConfiguration configuration)
        {
            _repository = repository;
            _imageUploaderService = imageUploaderService;
            _configuration = configuration;
        }

        public async Task<ApiResponse<MenuItemDto>> CreateMenuItemAsync(int tenantId, CreateMenuItemDto dto)
        {
            try
            {
                if (tenantId <= 0)
                {
                    return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                        "Invalid tenant ID",
                        new List<string> { "Tenant ID must be greater than 0" });
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

                var category = await _repository.GetCategoryByIdAsync(tenantId, dto.CategoryId);
                if (category == null)
                {
                    return ApiResponse<MenuItemDto>.ValidationErrorResponse(
                        "Invalid category",
                        new List<string> { $"Category with ID {dto.CategoryId} not found for this tenant" });
                }

                var exists = await _repository.MenuItemExistsAsync(tenantId, dto.Name, dto.CategoryId);
                if (exists)
                {
                    return ApiResponse<MenuItemDto>.ConflictResponse(
                        $"Menu item '{dto.Name}' already exists for this category");
                }

                string? imageUrl = null;
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

                var menuItem = new MenuItem
                {
                    TenantId = tenantId,
                    CategoryId = dto.CategoryId,
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    ImageUrl = imageUrl,
                    IsVeg = dto.IsVeg,
                    PreparationTime = dto.PreparationTime,
                    DisplayOrder = dto.DisplayOrder,
                    IsAvailable = dto.IsAvailable,
                    IsActive = dto.IsActive,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var created = await _repository.CreateMenuItemAsync(menuItem);

                var result = new MenuItemDto
                {
                    Id = created.Id,
                    TenantId = created.TenantId,
                    CategoryId = created.CategoryId,
                    CategoryName = category.Name,
                    Name = created.Name,
                    Description = created.Description,
                    Price = created.Price,
                    ImageUrl = created.ImageUrl,
                    IsVeg = created.IsVeg,
                    PreparationTime = created.PreparationTime,
                    DisplayOrder = created.DisplayOrder,
                    IsAvailable = created.IsAvailable,
                    IsActive = created.IsActive,
                    CreatedAt = created.CreatedAt,
                    UpdatedAt = created.UpdatedAt
                };

                return ApiResponse<MenuItemDto>.CreatedResponse(result, "Menu item created successfully");
            }
            catch
            {
                return ApiResponse<MenuItemDto>.ServerErrorResponse(
                    "An error occurred while creating the menu item. Please try again later.");
            }
        }
    }
}

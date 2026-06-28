using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Categories.UpdateCategory;
using Restaurant.Application.Common;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Categories.UpdateCategory
{
    public class UpdateCategoryService : IUpdateCategoryService
    {
        private readonly IUpdateCategoryRepository _repository;

        public UpdateCategoryService(IUpdateCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<CategoryDto>> UpdateCategoryAsync(int tenantId, int categoryId, UpdateCategoryDto dto)
        {
            try
            {
                if (tenantId <= 0)
                {
                    return ApiResponse<CategoryDto>.ValidationErrorResponse(
                        "Invalid tenant ID",
                        new List<string> { "Tenant ID must be greater than 0" });
                }

                if (categoryId <= 0)
                {
                    return ApiResponse<CategoryDto>.ValidationErrorResponse(
                        "Invalid category ID",
                        new List<string> { "Category ID must be greater than 0" });
                }

                var validationErrors = new List<string>();
                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    validationErrors.Add("Name is required");
                }

                if (dto.DisplayOrder.HasValue && dto.DisplayOrder.Value < 0)
                {
                    validationErrors.Add("DisplayOrder must be non-negative");
                }

                if (validationErrors.Count > 0)
                {
                    return ApiResponse<CategoryDto>.ValidationErrorResponse("Validation failed", validationErrors);
                }

                var existing = await _repository.GetCategoryByIdAsync(tenantId, categoryId);
                if (existing == null)
                {
                    return ApiResponse<CategoryDto>.NotFoundResponse($"Category with ID {categoryId} not found");
                }

                if (existing.Name != dto.Name)
                {
                    var duplicate = await _repository.GetCategoryByNameAsync(tenantId, dto.Name, categoryId);
                    if (duplicate != null)
                    {
                        return ApiResponse<CategoryDto>.ConflictResponse(
                            $"Category name '{dto.Name}' already exists for this tenant");
                    }
                }

                existing.Name = dto.Name;
                existing.Description = dto.Description;
                existing.ImageUrl = dto.ImageUrl;
                existing.DisplayOrder = dto.DisplayOrder;
                existing.Slug = dto.Slug;
                existing.IsActive = dto.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;

                var updated = await _repository.UpdateCategoryAsync(existing);
                if (!updated)
                {
                    return ApiResponse<CategoryDto>.ServerErrorResponse("Failed to update category. Please try again later.");
                }

                var dtoOut = new CategoryDto
                {
                    Id = existing.Id,
                    TenantId = existing.TenantId,
                    Name = existing.Name,
                    Description = existing.Description,
                    ImageUrl = existing.ImageUrl,
                    DisplayOrder = existing.DisplayOrder,
                    Slug = existing.Slug,
                    IsActive = existing.IsActive,
                    CreatedAt = existing.CreatedAt,
                    UpdatedAt = existing.UpdatedAt
                };

                return ApiResponse<CategoryDto>.SuccessResponse(dtoOut, "Category updated successfully");
            }
            catch
            {
                return ApiResponse<CategoryDto>.ServerErrorResponse("An error occurred while updating the category. Please try again later.");
            }
        }
    }
}

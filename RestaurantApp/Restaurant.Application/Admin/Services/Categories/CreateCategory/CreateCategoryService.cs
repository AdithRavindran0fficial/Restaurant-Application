using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Categories.CreateCategory;
using Restaurant.Application.Common;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Categories.CreateCategory
{
    public class CreateCategoryService : ICreateCategoryService
    {
        private readonly ICreateCategoryRepository _repository;

        public CreateCategoryService(ICreateCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<CategoryDto>> CreateCategoryAsync(int tenantId, CreateCategoryDto dto)
        {
            try
            {
                if (tenantId <= 0)
                {
                    return ApiResponse<CategoryDto>.ValidationErrorResponse(
                        "Invalid tenant ID",
                        new List<string> { "Tenant ID must be greater than 0" });
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return ApiResponse<CategoryDto>.ValidationErrorResponse(
                        "Validation failed",
                        new List<string> { "Name is required" });
                }

                var exists = await _repository.CategoryExistsAsync(tenantId, dto.Name);
                if (exists)
                {
                    return ApiResponse<CategoryDto>.ConflictResponse($"Category '{dto.Name}' already exists for this tenant");
                }

                var category = new Category
                {
                    TenantId = tenantId,
                    Name = dto.Name,
                    Description = dto.Description,
                    ImageUrl = dto.ImageUrl,
                    DisplayOrder = dto.DisplayOrder,
                    Slug = dto.Slug,
                    IsActive = dto.IsActive,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var created = await _repository.CreateCategoryAsync(category);

                var resultDto = new CategoryDto
                {
                    Id = created.Id,
                    TenantId = created.TenantId,
                    Name = created.Name,
                    Description = created.Description,
                    ImageUrl = created.ImageUrl,
                    DisplayOrder = created.DisplayOrder,
                    Slug = created.Slug,
                    IsActive = created.IsActive,
                    CreatedAt = created.CreatedAt,
                    UpdatedAt = created.UpdatedAt
                };

                return ApiResponse<CategoryDto>.CreatedResponse(resultDto, "Category created successfully");
            }
            catch
            {
                return ApiResponse<CategoryDto>.ServerErrorResponse("An error occurred while creating the category. Please try again later.");
            }
        }
    }
}

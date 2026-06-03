using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Categories.GetCategoryById;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Categories.GetCategoryById
{
    public class GetCategoryByIdService : IGetCategoryByIdService
    {
        private readonly IGetCategoryByIdRepository _repository;

        public GetCategoryByIdService(IGetCategoryByIdRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<CategoryDto>> GetCategoryByIdAsync(int tenantId, int categoryId)
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

            var category = await _repository.GetCategoryByIdAsync(tenantId, categoryId);

            if (category == null)
            {
                return ApiResponse<CategoryDto>.NotFoundResponse($"Category with ID {categoryId} not found");
            }

            var dto = new CategoryDto
            {
                Id = category.Id,
                TenantId = category.TenantId,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                DisplayOrder = category.DisplayOrder,
                Slug = category.Slug,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return ApiResponse<CategoryDto>.SuccessResponse(dto, "Category retrieved successfully");
        }
    }
}

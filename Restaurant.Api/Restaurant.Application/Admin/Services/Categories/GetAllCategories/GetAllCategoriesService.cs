using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Categories.GetAllCategories;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Categories.GetAllCategories
{
    public class GetAllCategoriesService : IGetAllCategoriesService
    {
        private readonly IGetAllCategoriesRepository _repository;

        public GetAllCategoriesService(IGetAllCategoriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetAllCategoriesAsync(int tenantId)
        {
            if (tenantId <= 0)
            {
                return ApiResponse<List<CategoryDto>>.ValidationErrorResponse(
                    "Invalid tenant ID",
                    new List<string> { "Tenant ID must be greater than 0" });
            }

            var categories = await _repository.GetAllCategoriesAsync(tenantId);

            var dtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                TenantId = c.TenantId,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                DisplayOrder = c.DisplayOrder,
                Slug = c.Slug,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return ApiResponse<List<CategoryDto>>.SuccessResponse(dtos, $"{dtos.Count} category(s) retrieved successfully");
        }
    }
}

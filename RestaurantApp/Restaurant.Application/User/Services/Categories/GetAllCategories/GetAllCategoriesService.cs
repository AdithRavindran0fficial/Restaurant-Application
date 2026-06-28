using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using Restaurant.Application.User.Interfaces.Categories.GetAllCategories;
using Restaurant.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.User.Services.Categories.GetAllCategories
{
    public class GetAllCategoriesService : IGetAllCategoriesService
    {
        private readonly IGetAllCategoriesRepository _repository;

        public GetAllCategoriesService(IGetAllCategoriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetAllCategoriesAsync(string qrToken)
        {
            if(string.IsNullOrEmpty(qrToken))
            {

                return  ApiResponse<List<CategoryDto>>.ValidationErrorResponse("QR token is required");
            }
            var table = await _repository.GetDiningTableByQrTokenAsync(qrToken);
            if (table == null)
            {
                return ApiResponse<List<CategoryDto>>.NotFoundResponse("Table not found");
            }

            var categories = await _repository.GetAllCategoriesAsync(table.TenantId);

            var categoryDto = categories.Select(c => new CategoryDto
            {
                CreatedAt = c.CreatedAt,
                Description = c.Description,
                DisplayOrder = c.DisplayOrder,
                Id = c.Id,
                ImageUrl = c.ImageUrl,
                Name = c.Name,
                IsActive = c.IsActive,
                Slug = c.Slug,
                TenantId = c.TenantId,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return ApiResponse<List<CategoryDto>>.SuccessResponse(categoryDto);




        }
    }
}

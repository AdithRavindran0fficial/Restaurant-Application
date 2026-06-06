using Restaurant.Application.Admin.Interfaces.Categories.DeleteCategory;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Categories.DeleteCategory
{
    public class DeleteCategoryService : IDeleteCategoryService
    {
        private readonly IDeleteCategoryRepository _repository;

        public DeleteCategoryService(IDeleteCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> DeleteCategoryAsync(int tenantId, int categoryId)
        {
            try
            {
                if (tenantId <= 0)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Invalid tenant ID",
                        new List<string> { "Tenant ID must be greater than 0" });
                }

                if (categoryId <= 0)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Invalid category ID",
                        new List<string> { "Category ID must be greater than 0" });
                }

                var category = await _repository.GetCategoryByIdAsync(tenantId, categoryId);

                if (category == null)
                {
                    return ApiResponse<bool>.NotFoundResponse($"Category with ID {categoryId} not found");
                }

                if (category.IsDeleted)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Category already deleted",
                        new List<string> { $"Category with ID {categoryId} is already marked as deleted" });
                }

                var result = await _repository.SoftDeleteCategoryAsync(category);

                if (!result)
                {
                    return ApiResponse<bool>.ServerErrorResponse("Failed to delete category. Please try again later.");
                }

                return ApiResponse<bool>.SuccessResponse(true, "Category deleted successfully");
            }
            catch
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "An error occurred while deleting the category. Please try again later.");
            }
        }
    }
}

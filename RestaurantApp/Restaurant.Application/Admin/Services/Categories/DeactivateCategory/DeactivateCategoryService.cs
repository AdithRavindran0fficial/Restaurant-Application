using Restaurant.Application.Admin.Interfaces.Categories.DeactivateCategory;
using Restaurant.Application.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Services.Categories.DeactivateCategory
{
    public class DeactivateCategoryService : IDeactivateCategoryService
    {
        private readonly IDeactivateCategoryRepository _repository;

        public DeactivateCategoryService(IDeactivateCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> DeactivateCategoryAsync(int tenantId, int categoryId)
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
                        "Cannot deactivate deleted category",
                        new List<string> { $"Category with ID {categoryId} is marked as deleted" });
                }

                if (!category.IsActive)
                {
                    return ApiResponse<bool>.ValidationErrorResponse(
                        "Category already inactive",
                        new List<string> { $"Category with ID {categoryId} is already inactive" });
                }

                var result = await _repository.DeactivateCategoryAsync(category);

                if (!result)
                {
                    return ApiResponse<bool>.ServerErrorResponse("Failed to deactivate category. Please try again later.");
                }

                return ApiResponse<bool>.SuccessResponse(true, "Category deactivated successfully");
            }
            catch
            {
                return ApiResponse<bool>.ServerErrorResponse(
                    "An error occurred while deactivating the category. Please try again later.");
            }
        }
    }
}

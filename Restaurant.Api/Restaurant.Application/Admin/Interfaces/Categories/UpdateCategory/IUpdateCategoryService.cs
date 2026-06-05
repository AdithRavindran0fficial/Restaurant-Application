using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.UpdateCategory
{
    public interface IUpdateCategoryService
    {
        Task<ApiResponse<CategoryDto>> UpdateCategoryAsync(int tenantId, int categoryId, UpdateCategoryDto dto);
    }
}

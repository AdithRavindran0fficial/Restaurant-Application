using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.CreateCategory
{
    public interface ICreateCategoryService
    {
        Task<ApiResponse<CategoryDto>> CreateCategoryAsync(int tenantId, CreateCategoryDto dto);
    }
}

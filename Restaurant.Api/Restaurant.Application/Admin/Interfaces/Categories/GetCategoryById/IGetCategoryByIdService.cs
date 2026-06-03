using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.GetCategoryById
{
    public interface IGetCategoryByIdService
    {
        Task<ApiResponse<CategoryDto>> GetCategoryByIdAsync(int tenantId, int categoryId);
    }
}

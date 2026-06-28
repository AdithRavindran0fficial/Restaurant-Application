using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.GetAllCategories
{
    public interface IGetAllCategoriesService
    {
        Task<ApiResponse<List<CategoryDto>>> GetAllCategoriesAsync(int tenantId);
    }
}

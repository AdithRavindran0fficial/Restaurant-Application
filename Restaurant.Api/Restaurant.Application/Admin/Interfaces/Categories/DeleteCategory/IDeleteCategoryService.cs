using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.DeleteCategory
{
    public interface IDeleteCategoryService
    {
        Task<ApiResponse<bool>> DeleteCategoryAsync(int tenantId, int categoryId);
    }
}

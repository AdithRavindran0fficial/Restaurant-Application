using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.ActivateCategory
{
    public interface IActivateCategoryService
    {
        Task<ApiResponse<bool>> ActivateCategoryAsync(int tenantId, int categoryId);
    }
}

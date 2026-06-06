using Restaurant.Application.Common;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.DeactivateCategory
{
    public interface IDeactivateCategoryService
    {
        Task<ApiResponse<bool>> DeactivateCategoryAsync(int tenantId, int categoryId);
    }
}

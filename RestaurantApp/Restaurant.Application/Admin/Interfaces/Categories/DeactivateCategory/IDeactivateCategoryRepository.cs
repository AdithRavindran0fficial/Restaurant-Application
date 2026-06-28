using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.DeactivateCategory
{
    public interface IDeactivateCategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId);
        Task<bool> DeactivateCategoryAsync(Category category);
    }
}

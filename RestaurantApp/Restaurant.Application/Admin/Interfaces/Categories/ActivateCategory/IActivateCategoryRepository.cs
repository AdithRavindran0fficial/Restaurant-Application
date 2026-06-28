using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.ActivateCategory
{
    public interface IActivateCategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId);
        Task<bool> ActivateCategoryAsync(Category category);
    }
}

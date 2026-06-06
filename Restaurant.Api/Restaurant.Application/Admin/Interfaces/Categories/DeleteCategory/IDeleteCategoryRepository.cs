using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.DeleteCategory
{
    public interface IDeleteCategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId);
        Task<bool> SoftDeleteCategoryAsync(Category category);
    }
}

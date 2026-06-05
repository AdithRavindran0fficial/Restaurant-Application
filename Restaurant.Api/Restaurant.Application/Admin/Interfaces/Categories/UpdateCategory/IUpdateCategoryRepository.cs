using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.UpdateCategory
{
    public interface IUpdateCategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId);
        Task<Category?> GetCategoryByNameAsync(int tenantId, string name, int excludeId);
        Task<bool> UpdateCategoryAsync(Category category);
    }
}

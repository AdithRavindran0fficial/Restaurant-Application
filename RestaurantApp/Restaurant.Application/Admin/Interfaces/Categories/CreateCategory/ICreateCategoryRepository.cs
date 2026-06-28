using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.CreateCategory
{
    public interface ICreateCategoryRepository
    {
        Task<bool> CategoryExistsAsync(int tenantId, string name);
        Task<Category> CreateCategoryAsync(Category category);
    }
}

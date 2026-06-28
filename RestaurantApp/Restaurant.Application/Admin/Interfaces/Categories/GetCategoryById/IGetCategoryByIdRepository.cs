using Restaurant.Domain.Entities;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.GetCategoryById
{
    public interface IGetCategoryByIdRepository
    {
        Task<Category?> GetCategoryByIdAsync(int tenantId, int categoryId);
    }
}

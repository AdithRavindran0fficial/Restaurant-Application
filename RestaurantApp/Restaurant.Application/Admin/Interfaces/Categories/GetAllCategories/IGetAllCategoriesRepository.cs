using Restaurant.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Categories.GetAllCategories
{
    public interface IGetAllCategoriesRepository
    {
        Task<List<Category>> GetAllCategoriesAsync(int tenantId);
    }
}

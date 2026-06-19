using Restaurant.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.User.Interfaces.Categories.GetAllCategories
{
    public interface IGetAllCategoriesRepository
    {
        Task<List<Category>> GetAllCategoriesAsync(int tenantId);
        Task<DiningTable?> GetDiningTableByQrTokenAsync(string qrToken);
    }
}

using Restaurant.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.GetMenuItemsByCategoryId
{
    public interface IGetMenuItemsByCategoryIdRepository
    {
        Task<List<MenuItem>> GetMenuItemsByCategoryIdAsync(int tenantId, int categoryId);
    }
}

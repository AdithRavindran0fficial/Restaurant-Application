using Restaurant.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.MenuItems.GetAllMenuItems
{
    public interface IGetAllMenuItemsRepository
    {
        Task<List<MenuItem>> GetAllMenuItemsAsync(int tenantId);
    }
}

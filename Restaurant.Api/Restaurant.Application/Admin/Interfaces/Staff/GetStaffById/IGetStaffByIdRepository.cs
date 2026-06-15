using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Staff.GetStaffById
{
    public interface IGetStaffByIdRepository
    {
        Task<Restaurant.Domain.Entities.Staff?> GetStaffByIdAsync(int tenantId, int staffId);
    }
}

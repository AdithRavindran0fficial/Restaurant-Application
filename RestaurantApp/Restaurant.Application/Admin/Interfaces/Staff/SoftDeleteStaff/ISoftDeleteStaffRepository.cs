using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Staff.SoftDeleteStaff
{
    public interface ISoftDeleteStaffRepository
    {
        Task<Restaurant.Domain.Entities.Staff?> GetStaffByIdAsync(int tenantId, int staffId);
        Task<bool> SoftDeleteStaffAsync(Restaurant.Domain.Entities.Staff staff);
    }
}

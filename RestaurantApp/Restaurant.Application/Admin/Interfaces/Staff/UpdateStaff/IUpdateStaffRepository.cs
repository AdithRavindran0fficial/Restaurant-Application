using System.Threading.Tasks;

namespace Restaurant.Application.Admin.Interfaces.Staff.UpdateStaff
{
    public interface IUpdateStaffRepository
    {
        Task<Restaurant.Domain.Entities.Staff?> GetStaffByIdAsync(int tenantId, int staffId);
        Task<Restaurant.Domain.Entities.Role?> GetRoleByIdAsync(int roleId);
        Task<bool> StaffEmailExistsAsync(int tenantId, string email, int excludeStaffId);
        Task<bool> UpdateStaffAsync(Restaurant.Domain.Entities.Staff staff);
    }
}

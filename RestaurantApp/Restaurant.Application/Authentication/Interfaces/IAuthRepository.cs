using Restaurant.Domain.Entities;

namespace Restaurant.Application.Authentication.Interfaces;

public interface IAuthRepository
{
    Task<Restaurant.Domain.Entities.SuperAdmin?> GetSuperAdminByEmailAsync(string email);
    Task<Staff?> GetStaffByEmailAsync(string email);
    Task UpdateStaffAsync(Staff staff);
    Task<Role?> GetRoleByIdAsync(int roleId);
}

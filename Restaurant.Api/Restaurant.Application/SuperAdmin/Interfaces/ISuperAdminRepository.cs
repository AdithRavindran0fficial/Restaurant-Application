using Restaurant.Domain.Entities;

namespace Restaurant.Application.SuperAdmin.Interfaces;

public interface ISuperAdminRepository
{
    Task<Restaurant.Domain.Entities.SuperAdmin?> GetSuperAdminByIdAsync(int superAdminId);
    Task UpdateSuperAdminAsync(Restaurant.Domain.Entities.SuperAdmin superAdmin);
}

namespace Restaurant.Application.Authentication.Interfaces;

public interface IJwtService
{
    string GenerateToken(int userId, string email, string firstName, string role, int? tenantId);
    bool ValidateToken(string token);
    string RefreshToken(string token);
    int? ExtractUserIdFromToken(string token);
}

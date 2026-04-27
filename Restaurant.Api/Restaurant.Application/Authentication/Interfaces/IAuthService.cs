using Restaurant.Application.Authentication.DTOs;
using Restaurant.Application.Common;

namespace Restaurant.Application.Authentication.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<ApiResponse<object>> LogoutAsync(int userId, string role);
    Task<ApiResponse<object>> RefreshTokenAsync(string token);
}

using Restaurant.Application.Authentication.DTOs;
using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Application.Common;

namespace Restaurant.Application.Authentication.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IAuthRepository authRepository, IJwtService jwtService)
    {
        _authRepository = authRepository;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        // 1. Validate input
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return ApiResponse<LoginResponseDto>.ValidationErrorResponse(
                "Email is required",
                new List<string> { "Email field cannot be empty" });
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return ApiResponse<LoginResponseDto>.ValidationErrorResponse(
                "Password is required",
                new List<string> { "Password field cannot be empty" });
        }

        // 2. Check SuperAdmin table first
        var superAdmin = await _authRepository.GetSuperAdminByEmailAsync(request.Email);

        if (superAdmin != null)
        {
            // SuperAdmin found - verify password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, superAdmin.PasswordHash);

            if (!isPasswordValid)
            {
                // Wrong password - STOP here, don't check Staff table
                return ApiResponse<LoginResponseDto>.UnauthorizedResponse("Invalid credentials");
            }

            // Valid SuperAdmin login - generate token
            var token = _jwtService.GenerateToken(
                superAdmin.Id,
                superAdmin.Email,
                superAdmin.FirstName,
                "SuperAdmin",
                null  // tenantId is null for SuperAdmin
            );

            var response = new LoginResponseDto
            {
                Token = token,
                Role = "SuperAdmin",
                TenantId = null,
                FirstName = superAdmin.FirstName,
                Email = superAdmin.Email
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful");
        }

        // 3. SuperAdmin not found - Check Staff table
        var staff = await _authRepository.GetStaffByEmailAsync(request.Email);

        if (staff == null)
        {
            // Email not found in Staff table
            return ApiResponse<LoginResponseDto>.UnauthorizedResponse("Invalid credentials");
        }

        // 4. Staff found - Check account status
        if (!staff.IsActive)
        {
            return ApiResponse<LoginResponseDto>.UnauthorizedResponse("Account is disabled");
        }

        // 5. Check if account is locked
        if (staff.LockedUntil.HasValue && staff.LockedUntil.Value > DateTime.UtcNow)
        {
            var remainingMinutes = (int)(staff.LockedUntil.Value - DateTime.UtcNow).TotalMinutes;
            return ApiResponse<LoginResponseDto>.UnauthorizedResponse(
                $"Account locked. Try after {remainingMinutes} minutes");
        }

        // 6. Verify password
        bool isStaffPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, staff.PasswordHash);

        if (!isStaffPasswordValid)
        {
            // Wrong password - increment failed attempts
            staff.FailedLoginAttempts++;

            if (staff.FailedLoginAttempts >= 5)
            {
                // Lock account for 15 minutes
                staff.LockedUntil = DateTime.UtcNow.AddMinutes(15);
            }

            staff.UpdatedAt = DateTime.UtcNow;
            await _authRepository.UpdateStaffAsync(staff);

            return ApiResponse<LoginResponseDto>.UnauthorizedResponse("Invalid credentials");
        }

        // 7. Valid Staff login - reset failed attempts and generate token
        staff.FailedLoginAttempts = 0;
        staff.LockedUntil = null;
        staff.LastLoginAt = DateTime.UtcNow;
        staff.UpdatedAt = DateTime.UtcNow;
        await _authRepository.UpdateStaffAsync(staff);

        var staffToken = _jwtService.GenerateToken(
            staff.Id,
            staff.Email,
            staff.FirstName,
            staff.Role.RoleName,
            staff.TenantId
        );

        var staffResponse = new LoginResponseDto
        {
            Token = staffToken,
            Role = staff.Role.RoleName,
            TenantId = staff.TenantId,
            FirstName = staff.FirstName,
            Email = staff.Email
        };

        return ApiResponse<LoginResponseDto>.SuccessResponse(staffResponse, "Login successful");
    }

    public async Task<ApiResponse<object>> LogoutAsync(int userId, string role)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<object>> RefreshTokenAsync(string token)
    {
        throw new NotImplementedException();
    }
}

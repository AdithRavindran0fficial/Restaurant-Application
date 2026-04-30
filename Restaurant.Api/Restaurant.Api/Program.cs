using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Application.Authentication.Services;
using Restaurant.Application.SuperAdmin.Interfaces;
using Restaurant.Application.SuperAdmin.Services;
using Restaurant.Application.SuperAdmin.Interfaces.GetAllTenants;
using Restaurant.Application.SuperAdmin.Services.GetAllTenants;
using Restaurnat.Infra.Authentication;
using Restaurnat.Infra.Context;
using Restaurnat.Infra.SuperAdmin;
using Restaurnat.Infra.SuperAdmin.GetAllTenants;
using Scalar.AspNetCore;
using System.Text;

namespace Restaurant.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Database ───────────────────────────────────────────────
            builder.Services.AddDbContext<MasterDbContext>(options =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // ── Repositories ───────────────────────────────────────────
            builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            builder.Services.AddScoped<ISuperAdminRepository, SuperAdminRepository>();
            builder.Services.AddScoped<ITenantRepository, TenantRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            // ── Services ───────────────────────────────────────────────
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ISuperAdminService, SuperAdminService>();
            builder.Services.AddScoped<ITenantService, TenantService>();

            // ── Controllers ────────────────────────────────────────────
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer(); // ← ADD THIS

            // ── OpenAPI ────────────────────────────────────────────────
            builder.Services.AddOpenApi();

            // ── JWT Authentication ─────────────────────────────────────
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            builder.Services.AddAuthorization();

            // ── Build ──────────────────────────────────────────────────
            var app = builder.Build();

            // ── Middleware ─────────────────────────────────────────────
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "Restaurant SaaS API";
                    options.Theme = ScalarTheme.DeepSpace;
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
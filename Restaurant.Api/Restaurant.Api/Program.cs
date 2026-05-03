using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.Authentication.Interfaces;
using Restaurant.Application.Authentication.Services;
using Restaurant.Application.SuperAdmin.Interfaces;
using Restaurant.Application.SuperAdmin.Services;
using Restaurant.Application.SuperAdmin.Interfaces.GetAllTenants;
using Restaurant.Application.SuperAdmin.Services.GetAllTenants;
using Restaurant.Application.SuperAdmin.Interfaces.SoftDeleteTenant;
using Restaurant.Application.SuperAdmin.Services.SoftDeleteTenant;
using Restaurant.Application.SuperAdmin.Interfaces.ActivateTenant;
using Restaurant.Application.SuperAdmin.Services.ActivateTenant;
using Restaurant.Application.SuperAdmin.Interfaces.DeactivateTenant;
using Restaurant.Application.SuperAdmin.Services.DeactivateTenant;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;
using Restaurant.Application.SuperAdmin.Services.Subscription.GetAllSubscriptions;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;
using Restaurant.Application.SuperAdmin.Services.Subscription.CreateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.UpdateSubscription;
using Restaurant.Application.SuperAdmin.Services.Subscription.UpdateSubscription;
using Restaurnat.Infra.Authentication;
using Restaurnat.Infra.Context;
using Restaurnat.Infra.SuperAdmin;
using Restaurnat.Infra.SuperAdmin.GetAllTenants;
using Restaurnat.Infra.SuperAdmin.SoftDeleteTenant;
using Restaurnat.Infra.SuperAdmin.ActivateTenant;
using Restaurnat.Infra.SuperAdmin.DeactivateTenant;
using Restaurnat.Infra.SuperAdmin.Subscription.GetAllSubscriptions;
using Restaurnat.Infra.SuperAdmin.Subscription.CreateSubscription;
using Restaurnat.Infra.SuperAdmin.Subscription.UpdateSubscription;
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
            builder.Services.AddScoped<ISoftDeleteTenantService, SoftDeleteTenantService>();
            builder.Services.AddScoped<IActivateTenantService, ActivateTenantService>();
            builder.Services.AddScoped<IDeactivateTenantService, DeactivateTenantService>();
            builder.Services.AddScoped<IGetAllSubscriptionsService, GetAllSubscriptionsService>();
            builder.Services.AddScoped<ICreateSubscriptionService, CreateSubscriptionService>();
            builder.Services.AddScoped<IUpdateSubscriptionService, UpdateSubscriptionService>();

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
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
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetSubscriptionById;
using Restaurant.Application.SuperAdmin.Services.Subscription.GetSubscriptionById;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;
using Restaurant.Application.SuperAdmin.Services.Subscription.CreateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.UpdateSubscription;
using Restaurant.Application.SuperAdmin.Services.Subscription.UpdateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeleteSubscription;
using Restaurant.Application.SuperAdmin.Services.Subscription.DeleteSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.ActivateSubscription;
using Restaurant.Application.SuperAdmin.Services.Subscription.ActivateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeactivateSubscription;
using Restaurant.Application.SuperAdmin.Services.Subscription.DeactivateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.GetTenantSubscription;
using Restaurant.Application.SuperAdmin.Services.TenantSubscriptionManagement.GetTenantSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.AssignSubscription;
using Restaurant.Application.SuperAdmin.Services.TenantSubscriptionManagement.AssignSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.TenantSubscriptionManagement.CancelSubscription;
using Restaurant.Application.SuperAdmin.Services.TenantSubscriptionManagement.CancelSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.DashBoard;
using Restaurant.Application.SuperAdmin.Services.DashBoard;
using Restaurnat.Infra.SuperAdmin.DashBoard;
using Restaurnat.Infra.Authentication;
using Restaurnat.Infra.Context;
using Restaurnat.Infra.SuperAdmin;
using Restaurnat.Infra.SuperAdmin.GetAllTenants;
using Restaurnat.Infra.SuperAdmin.SoftDeleteTenant;
using Restaurnat.Infra.SuperAdmin.ActivateTenant;
using Restaurnat.Infra.SuperAdmin.DeactivateTenant;
using Restaurnat.Infra.SuperAdmin.Subscription.GetAllSubscriptions;
using Restaurnat.Infra.SuperAdmin.Subscription.GetSubscriptionById;
using Restaurnat.Infra.SuperAdmin.Subscription.CreateSubscription;
using Restaurnat.Infra.SuperAdmin.Subscription.UpdateSubscription;
using Restaurnat.Infra.SuperAdmin.Subscription.DeleteSubscription;
using Restaurnat.Infra.SuperAdmin.Subscription.ActivateSubscription;
using Restaurnat.Infra.SuperAdmin.Subscription.DeactivateSubscription;
using Restaurnat.Infra.SuperAdmin.TenantSubscriptionManagement.GetTenantSubscription;
using Restaurnat.Infra.SuperAdmin.TenantSubscriptionManagement.AssignSubscription;
using Restaurnat.Infra.SuperAdmin.TenantSubscriptionManagement.CancelSubscription;
using Restaurant.Application.Admin.Interfaces.Tables.GetAllTables;
using Restaurant.Application.Admin.Services.Tables.GetAllTables;
using Restaurant.Application.Admin.Interfaces.Tables.GetTableById;
using Restaurant.Application.Admin.Services.Tables.GetTableById;
using Restaurant.Application.Admin.Interfaces.Tables.CreateTable;
using Restaurant.Application.Admin.Services.Tables.CreateTable;
using Restaurant.Application.Admin.Interfaces.Tables.UpdateTable;
using Restaurant.Application.Admin.Services.Tables.UpdateTable;
using Restaurant.Application.Admin.Interfaces.Tables.SoftDeleteTable;
using Restaurant.Application.Admin.Services.Tables.SoftDeleteTable;
using Restaurant.Application.Admin.Interfaces.Tables.ActivateTable;
using Restaurant.Application.Admin.Services.Tables.ActivateTable;
using Restaurant.Application.Admin.Interfaces.Tables.DeactivateTable;
using Restaurant.Application.Admin.Services.Tables.DeactivateTable;
using Restaurnat.Infra.Admin.Tables.GetAllTables;
using Restaurnat.Infra.Admin.Tables.GetTableById;
using Restaurnat.Infra.Admin.Tables.CreateTable;
using Restaurnat.Infra.Admin.Tables.UpdateTable;
using Restaurnat.Infra.Admin.Tables.SoftDeleteTable;
using Restaurnat.Infra.Admin.Tables.ActivateTable;
using Restaurnat.Infra.Admin.Tables.DeactivateTable;
using Scalar.AspNetCore;
using System.Text;
using Restaurant.Application.Common.ImageServices;

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
            builder.Services.AddScoped<ISoftDeleteTenantRepository, SoftDeleteTenantRepository>();
            builder.Services.AddScoped<IActivateTenantRepository, ActivateTenantRepository>();
            builder.Services.AddScoped<IDeactivateTenantRepository, DeactivateTenantRepository>();
            builder.Services.AddScoped<IGetAllSubscriptionsRepository, GetAllSubscriptionsRepository>();
            builder.Services.AddScoped<IGetSubscriptionByIdRepository, GetSubscriptionByIdRepository>();
            builder.Services.AddScoped<ICreateSubscriptionRepository, CreateSubscriptionRepository>();
            builder.Services.AddScoped<IUpdateSubscriptionRepository, UpdateSubscriptionRepository>();
            builder.Services.AddScoped<IDeleteSubscriptionRepository, DeleteSubscriptionRepository>();
            builder.Services.AddScoped<IActivateSubscriptionRepository, ActivateSubscriptionRepository>();
            builder.Services.AddScoped<IDeactivateSubscriptionRepository, DeactivateSubscriptionRepository>();
            builder.Services.AddScoped<IGetTenantSubscriptionRepository, GetTenantSubscriptionRepository>();
            builder.Services.AddScoped<IAssignSubscriptionRepository, AssignSubscriptionRepository>();
            builder.Services.AddScoped<ICancelSubscriptionRepository, CancelSubscriptionRepository>();
            builder.Services.AddScoped<IDashBoardAnalyticsRepository, DashBoardAnalysticsRepository>();
            builder.Services.AddScoped<IGetAllTablesRepository, GetAllTablesRepository>();
            builder.Services.AddScoped<IGetTableByIdRepository, GetTableByIdRepository>();
            builder.Services.AddScoped<ICreateTableRepository, CreateTableRepository>();
            builder.Services.AddScoped<IUpdateTableRepository, UpdateTableRepository>();
            builder.Services.AddScoped<ISoftDeleteTableRepository, SoftDeleteTableRepository>();
            builder.Services.AddScoped<IActivateTableRepository, ActivateTableRepository>();
            builder.Services.AddScoped<IDeactivateTableRepository, DeactivateTableRepository>();
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
            builder.Services.AddScoped<IGetSubscriptionByIdService, GetSubscriptionByIdService>();
            builder.Services.AddScoped<ICreateSubscriptionService, CreateSubscriptionService>();
            builder.Services.AddScoped<IUpdateSubscriptionService, UpdateSubscriptionService>();
            builder.Services.AddScoped<IDeleteSubscriptionService, DeleteSubscriptionService>();
            builder.Services.AddScoped<IActivateSubscriptionService, ActivateSubscriptionService>();
            builder.Services.AddScoped<IDeactivateSubscriptionService, DeactivateSubscriptionService>();
            builder.Services.AddScoped<IGetTenantSubscriptionService, GetTenantSubscriptionService>();
            builder.Services.AddScoped<IAssignSubscriptionService, AssignSubscriptionService>();
            builder.Services.AddScoped<ICancelSubscriptionService, CancelSubscriptionService>();
            builder.Services.AddScoped<IDashBoardAnalyticsService, DashBoardAnalyticsService>();
            builder.Services.AddScoped<IGetAllTablesService, GetAllTablesService>();
            builder.Services.AddScoped<IGetTableByIdService, GetTableByIdService>();
            builder.Services.AddScoped<ICreateTableService, CreateTableService>();
            builder.Services.AddScoped<IUpdateTableService, UpdateTableService>();
            builder.Services.AddScoped<ISoftDeleteTableService, SoftDeleteTableService>();
            builder.Services.AddScoped<IActivateTableService, ActivateTableService>();
            builder.Services.AddScoped<IDeactivateTableService, DeactivateTableService>();
            builder.Services.AddScoped<IImageUploaderService, ImageUploaderService>();

            // ── Controllers
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
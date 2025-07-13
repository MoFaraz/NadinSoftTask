using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Domain.Entities.User;
using NadinSoft.Infrastructure.Identity.IdentitySetup.Factories;
using NadinSoft.Infrastructure.Identity.IdentitySetup.Stores;
using NadinSoft.Infrastructure.Identity.Services.Implementations;
using NadinSoft.Infrastructure.Identity.Services.Models;
using NadinSoft.Infrastructure.Persistence;

namespace NadinSoft.Infrastructure.Identity.Extensions;

public static class IdentityServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserClaimsPrincipalFactory<UserEntity>, AppUserClaimPrincipalFactory>();
        services.AddScoped<IRoleStore<RoleEntity>, AppRoleStore>();
        services.AddScoped<IUserStore<UserEntity>, AppUserStore>();

        services.AddIdentity<UserEntity, RoleEntity>(opt =>
            {
                opt.Stores.ProtectPersonalData = false;

                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredUniqueChars = 0;

                opt.SignIn.RequireConfirmedEmail = false;
                opt.SignIn.RequireConfirmedPhoneNumber = false;

                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.AllowedForNewUsers = false;
                opt.User.RequireUniqueEmail = false;
            }).AddRoleStore<AppRoleStore>().AddUserStore<AppUserStore>()
            .AddClaimsPrincipalFactory<AppUserClaimPrincipalFactory>().AddDefaultTokenProviders()
            .AddEntityFrameworkStores<NadinSoftDbContext>();
        

        services.Configure<JwtConfiguration>(configuration.GetSection(nameof(JwtConfiguration)));

        services.AddScoped<IJwtService, JwtServiceImplementation>();
        services.AddScoped<IUserManager, UserManagerImplementation>();

        return services;
    }
}
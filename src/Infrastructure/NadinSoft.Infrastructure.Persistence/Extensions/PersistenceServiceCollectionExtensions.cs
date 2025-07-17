using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Application.Repositories.Common;
using NadinSoft.Infrastructure.Persistence.Interceptors;
using NadinSoft.Infrastructure.Persistence.Repositories.Common;

namespace NadinSoft.Infrastructure.Persistence.Extensions;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuditInterceptor>();

        services.AddDbContext<NadinSoftDbContext>((provider, opt) =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("NadinSoftDb"), builder =>
            {
                builder.EnableRetryOnFailure();
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }).AddInterceptors(provider.GetRequiredService<AuditInterceptor>());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
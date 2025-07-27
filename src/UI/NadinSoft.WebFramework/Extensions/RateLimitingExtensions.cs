using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NadinSoft.WebFramework.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddMemoryCache(); 
       services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

       services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
       services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
       services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
       services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
       services.AddHttpContextAccessor();

       return services;
    }
}
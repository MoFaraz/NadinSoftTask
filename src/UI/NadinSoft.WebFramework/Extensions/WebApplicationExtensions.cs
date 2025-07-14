using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NadinSoft.Infrastructure.Persistence;

namespace NadinSoft.WebFramework.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NadinSoftDbContext>();

        await db.Database.MigrateAsync();
    }
    
}
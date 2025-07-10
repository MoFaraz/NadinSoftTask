using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace NadinSoft.Infrastructure.Persistence.Extensions;

internal static class ModelBuilderExtensions
{
    public static void RegisterEntity<TEntity>(this ModelBuilder builder, params Assembly[] assemblies)
    {
        var entityType = assemblies.SelectMany(a => a.ExportedTypes)
            .Where(c => c is { IsClass: true, IsAbstract: false, IsPublic: true, IsGenericTypeDefinition: false } &&
                        typeof(TEntity).IsAssignableFrom(c));

        foreach (var type in entityType)
        {
            try
            {
                builder.Entity(type);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error registering {type.FullName}: {ex.Message}");
                throw;
            }

        }
    }

    public static void ApplyDeleteRestrictBehavior(this ModelBuilder builder)
    {
        var cascadeForeignKeys = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => fk is { IsOwnership: false, DeleteBehavior: DeleteBehavior.Restrict });
        
        foreach (var foreignKey in cascadeForeignKeys)
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
    }
}
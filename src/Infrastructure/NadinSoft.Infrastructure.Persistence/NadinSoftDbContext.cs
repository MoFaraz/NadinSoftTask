using Microsoft.EntityFrameworkCore;
using NadinSoft.Domain.Common;
using NadinSoft.Infrastructure.Persistence.Extensions;

namespace NadinSoft.Infrastructure.Persistence;

public class NadinSoftDbContext(DbContextOptions<NadinSoftDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterEntity<IEntity>(typeof(IEntity).Assembly);
        modelBuilder.ApplyDeleteRestrictBehavior();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NadinSoftDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Exception = System.Exception;

namespace NadinSoft.Infrastructure.Persistence;

public class NadinSoftDbContextDesignTimeFactory : IDesignTimeDbContextFactory<NadinSoftDbContext>
{
    public NadinSoftDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NadinSoftDbContext>();
        optionsBuilder.UseSqlServer();
        
        return new NadinSoftDbContext(optionsBuilder.Options);
    }
}
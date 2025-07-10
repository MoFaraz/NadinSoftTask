using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Exception = System.Exception;

namespace NadinSoft.Infrastructure.Persistence;

public class NadinSoftDbContextDesignTimeFactory : IDesignTimeDbContextFactory<NadinSoftDbContext>
{
    public NadinSoftDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            "Server=localhost,1433;Database=NadinSoftDb;User Id=sa;Password=Your_strong_password123!;TrustServerCertificate=True;";
        var optionsBuilder = new DbContextOptionsBuilder<NadinSoftDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        
        Console.WriteLine("Creating DbContext with connection string...");
        return new NadinSoftDbContext(optionsBuilder.Options);
    }
}
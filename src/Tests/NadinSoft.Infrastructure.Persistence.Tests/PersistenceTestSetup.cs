using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NadinSoft.Application.Extensions;
using NadinSoft.Infrastructure.Identity.Extensions;
using NadinSoft.Infrastructure.Persistence.Extensions;
using NadinSoft.Infrastructure.Persistence.Repositories.Common;
using Testcontainers.MsSql;

namespace NadinSoft.Infrastructure.Persistence.Tests;

public class PersistenceTestSetup : IAsyncLifetime
{
    public UnitOfWork UnitOfWork { get; private set; } = null!;

    private readonly MsSqlContainer _sqlContainer =
        new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server").Build();

    public IServiceProvider ServiceProvider { get; private set; }

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
        var dbOptionBuilder = new DbContextOptionsBuilder<NadinSoftDbContext>()
            .UseSqlServer(_sqlContainer.GetConnectionString());

        var db = new NadinSoftDbContext(dbOptionBuilder.Options);

        await db.Database.MigrateAsync();

        UnitOfWork = new UnitOfWork(db);

        var configs = new Dictionary<string, string>()
        {
            { "ConnectionStrings:NadinSoftDb", _sqlContainer.GetConnectionString() },
            { "JwtConfiguration:SignInKey", "ShouldBe-LongerThan-16Char-SecretKey" },
            { "JwtConfiguration:Audience", "TestAud" },
            { "JwtConfiguration:Issuer", "TestIssuer" },
            { "JwtConfiguration:ExpirationInMinutes", "60" },
        };

        var configurationBuilder = new ConfigurationBuilder();
        var inMemoryConfigs = new MemoryConfigurationSource() { InitialData = configs };

        configurationBuilder.Add(inMemoryConfigs);


        var serviceCollection = new ServiceCollection();
        serviceCollection.AddApplicationAutoMapper()
            .AddApplicationMediatorServices()
            .RegisterApplicationValidator()
            .AddPersistenceDbContext(configurationBuilder.Build())
            .AddIdentityServices(configurationBuilder.Build())
            .AddLogging(c => c.AddConsole());

        ServiceProvider = serviceCollection.BuildServiceProvider(false);
    }

    public async Task DisposeAsync()
    {
        await _sqlContainer.StopAsync();
    }
}
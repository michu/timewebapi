namespace TimeWebApi.IntegrationTests;

using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeWebApi;

public static class Fixture
{
    public static ServiceProvider ServiceProvider { get; private set; }

    static Fixture()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        var connectionString = config.GetSection("ConnectionStrings:TimeWebApi").Value!;

        var services = new ServiceCollection();

        services.AddNpgsql(connectionString);
        services.AddFluentMigration(connectionString);
        services.AddRepositories();
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddMediator();

        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<IMigrationRunner>()
                .MigrateUp();
        }

        ServiceProvider = serviceProvider;
    }
}

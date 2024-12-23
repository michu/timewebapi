namespace TimeWebApi.IntegrationTests;

using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using System.Threading.Tasks;

public abstract class IntegrationTestBase : IAsyncLifetime
{
    public IServiceScope Scope { get; private set; }

    public IntegrationTestBase()
    {
        Scope = Fixture.ServiceProvider.CreateScope();
    }

    public async Task InitializeAsync()
    {
        var connection = Scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();

        var respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = ["VersionInfo"]
        });

        await respawner.ResetAsync(connection);
    }

    public async Task DisposeAsync()
    {
        await Task.CompletedTask;

        Scope.Dispose();
    }

    protected static async Task AddEmployeeToDatabase(NpgsqlConnection connection, EmployeeData employee)
            => await connection.ExecuteAsync(new CommandDefinition(@"
INSERT INTO ""Employees"" (
    ""Email"",
    ""FirstName"",
    ""Id"",
    ""LastName""
) VALUES (
    @Email,
    @FirstName,
    @Id,
    @LastName
)",
            parameters: employee));

    protected record EmployeeData(int Id, string Email, string FirstName, string LastName);
}

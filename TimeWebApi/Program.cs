namespace TimeWebApi;

using Dapper;
using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TimeWebApi.Middlewares;
using TimeWebApi.Resources;
using TimeWebApi.TypeHandlers;

public class Program
{
    public static void Main(string[] args)
    {
        // Add dapper mappings.
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        // Create builder.
        var builder = WebApplication.CreateBuilder(args);

        // Configure logging.
        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString(StaticData.ConnectionStrings.DefaultConnectionString)!;
        var jwtIssuer = builder.Configuration.GetSection(StaticData.ConfigurationOptions.JwtIssuer).Get<string>()!;
        var jwtKey = builder.Configuration.GetSection(StaticData.ConfigurationOptions.JwtKey).Get<string>()!;

        builder.Services.AddAuthentication(jwtIssuer, jwtKey);
        builder.Services.AddNpgsql(connectionString);
        builder.Services.AddFluentMigration(connectionString);
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        builder.Services.AddMediator();

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);

        // Add exception handlers.
        builder.Services.AddExceptionHandlers();

        // Add swagger.
        builder.Services.AddSwagger();

        var app = builder.Build();

        // Perform database migrate up.
        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<IMigrationRunner>()
                .MigrateUp();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseExceptionHandler();

        app.UseMiddleware<EmployeeBasedAccessMiddleware>();
        app.UseMiddleware<ForbiddenMiddleware>();

        app.MapControllers();

        app.Run();
    }
}

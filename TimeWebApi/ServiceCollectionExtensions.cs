using TimeWebApi;

namespace TimeWebApi;

using FluentMigrator.Runner;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Text;
using TimeWebApi.Auth;
using TimeWebApi.Behaviours;
using TimeWebApi.ExceptionHandlers;

public static class ServiceCollectionExtensions
{
    public static void AddAuthentication(this IServiceCollection services, string jwtIssuer, string jwtKey)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        services.AddSingleton<JwtTokenGenerator>();
    }

    public static void AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<FeatureExceptionHandler>();
        services.AddExceptionHandler<ForbiddenExceptionHandler>();
        services.AddProblemDetails();
    }

    public static void AddFluentMigration(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres15_0()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole());
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
    }

    public static void AddNpgsql(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(serviceProvider => new NpgsqlDataSourceBuilder(connectionString)
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .Build());

        services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<NpgsqlDataSource>()
            .OpenConnection());
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });

            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml"));

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TimeWebAPI",
                Version = "v1",
                Description = "An API to perform CRUD operations on employees and theirs time entries entities",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Michał Piszczek",
                    Email = "michalp@op.pl",
                    Url = new Uri("https://github.com/michu"),
                },
                License = new OpenApiLicense
                {
                    Name = "Example Licence",
                    Url = new Uri("https://example.com/license"),
                }
            });
        });
    }
}

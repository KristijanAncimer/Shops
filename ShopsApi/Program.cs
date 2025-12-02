using Auth.Infrastructure;
using Core.Health;
using ShopsApi.Endpoints;
using ShopsApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

// Configure application settings
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

// Configure logging
builder.ConfigureSerilog();

// Add services to the container
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddAppHealthChecks(builder.Configuration);
builder.Services.AddRateLimitingConfiguration();
builder.Services.AddDatabaseConfiguration(builder.Configuration, builder.Environment);
builder.Services.AddValidationConfiguration();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure middleware pipeline
app.ConfigureMiddleware();
await app.ConfigureDatabaseMigration();
app.UseSwaggerConfiguration();

// Map endpoints
app.MapAuthEndpoints();
app.MapShopEndpoints();
app.MapWeatherEndpoints();
app.MapHealthChecks("/health");

await app.RunAsync();

public partial class Program { }
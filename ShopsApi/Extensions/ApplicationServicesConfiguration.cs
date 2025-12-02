using Core.Integrations.Weather;
using Core.Middlewares;
using MediatR;
using Shops.Application;
using Shops.Application.Mappings;
using Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Configuration;
using Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Generators;

namespace ShopsApi.Extensions;

public static class ApplicationServicesConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
        services.AddAutoMapper(typeof(ShopMappingProfile).Assembly);
        services.AddWeatherIntegration();
        services.AddTransient<GlobalExceptionMiddleware>();

        // Configure weather report generators
        services.Configure<PdfReportSettings>(configuration.GetSection("PdfReportSettings"));
        services.AddScoped<IPdfWeatherReportGenerator, PdfWeatherReportGenerator>();
        services.AddScoped<IExcelWeatherReportGenerator, ExcelWeatherReportGenerator>();

        return services;
    }
}

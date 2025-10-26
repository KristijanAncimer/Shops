using Microsoft.Extensions.DependencyInjection;

namespace Core.Integrations.Weather;

public static class WeatherServiceRegistration
{
    public static IServiceCollection AddWeatherIntegration(this IServiceCollection services)
    {
        services.AddHttpClient<IWeatherIntegrationService, WeatherIntegrationService>();
        return services;
    }
}

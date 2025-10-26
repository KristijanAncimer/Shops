namespace Core.Integrations.Weather;

public interface IWeatherIntegrationService
{
    Task<WeatherData> GetWeatherDataAsync(decimal latitude, decimal longitude, int daysBack, CancellationToken cancellationToken);
}

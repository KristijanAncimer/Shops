using Core.Middlewares.Exceptions;
using System.Net.Http.Json;

namespace Core.Integrations.Weather;

public class WeatherIntegrationService : IWeatherIntegrationService
{
    private readonly HttpClient _httpClient;

    public WeatherIntegrationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherData> GetWeatherDataAsync(decimal latitude, decimal longitude, int daysBack, CancellationToken cancellationToken)
    {
        var endDate = DateTime.UtcNow.Date;
        var startDate = endDate.AddDays(-daysBack);

        var url = $"https://api.open-meteo.com/v1/forecast?" +
                  $"latitude={latitude}&longitude={longitude}" +
                  $"&daily=temperature_2m_max,temperature_2m_min,precipitation_sum,windspeed_10m_max" +
                  $"&timezone=auto" +
                  $"&start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}";

        var result = await _httpClient.GetFromJsonAsync<WeatherData>(url, cancellationToken);
        return result ?? throw new NotFoundException("Failed to retrieve weather data.");
    }
}

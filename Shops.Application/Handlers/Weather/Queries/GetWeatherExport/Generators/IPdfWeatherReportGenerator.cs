namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Generators;

public interface IPdfWeatherReportGenerator
{
    ExportWeatherHandlerDto Generate(List<WeatherDayDto> weatherData, DateTime generatedAt);
}

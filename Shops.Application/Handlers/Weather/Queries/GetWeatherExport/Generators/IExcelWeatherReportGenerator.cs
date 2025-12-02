namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Generators;

public interface IExcelWeatherReportGenerator
{
    ExportWeatherHandlerDto Generate(List<WeatherDayDto> weatherData, DateTime generatedAt);
}

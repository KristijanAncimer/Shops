using Core.Integrations.Weather;
using Core.Middlewares.Exceptions;
using MediatR;
using Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Generators;
using System.ComponentModel.DataAnnotations;

namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport;

public class ExportWeatherHandler : IRequestHandler<ExportWeatherHandlerRequest, ExportWeatherHandlerDto>
{
    private readonly IWeatherIntegrationService _weatherService;
    private readonly IPdfWeatherReportGenerator _pdfGenerator;
    private readonly IExcelWeatherReportGenerator _excelGenerator;

    public ExportWeatherHandler(
        IWeatherIntegrationService weatherService,
        IPdfWeatherReportGenerator pdfGenerator,
        IExcelWeatherReportGenerator excelGenerator)
    {
        _weatherService = weatherService;
        _pdfGenerator = pdfGenerator;
        _excelGenerator = excelGenerator;
    }

    public async Task<ExportWeatherHandlerDto> Handle(ExportWeatherHandlerRequest request, CancellationToken cancellationToken)
    {
        var daysBack = request.GetDaysBackOrDefault();
        var generatedAt = request.GeneratedAt ?? DateTime.UtcNow;
        var weather = await _weatherService.GetWeatherDataAsync(request.Latitude, request.Longitude, daysBack, cancellationToken);

        if (weather?.Daily?.Time == null || !weather.Daily.Time.Any())
            throw new NotFoundException("No weather data found for the given coordinates and date range.");

        var dailyData = weather.Daily.Time
            .Select((date, index) => new WeatherDayDto(
                date,
                weather.Daily.Temperature_2m_Max[index],
                weather.Daily.Temperature_2m_Min[index],
                weather.Daily.Windspeed_10m_Max[index],
                weather.Daily.Precipitation_Sum[index]
            ))
            .OrderByDescending(x => x.Date)
            .ToList();

        return request.Format switch
        {
            Format.Pdf => _pdfGenerator.Generate(dailyData, generatedAt),
            Format.Excel => _excelGenerator.Generate(dailyData, generatedAt),
            _ => throw new ValidationException("Invalid format. Must be Pdf or Excel.")
        };
    }
}

public record WeatherDayDto(string Date, decimal TempMax, decimal TempMin, decimal WindSpeed, decimal Precipitation);
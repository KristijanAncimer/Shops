using ClosedXML.Excel;
using Core.Integrations.Weather;
using Core.Middlewares.Exceptions;
using MediatR;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport;

public class ExportWeatherHandler : IRequestHandler<ExportWeatherHandlerRequest, ExportWeatherHandlerDto>
{
    private readonly IWeatherIntegrationService _weatherService;

    private const string PdfContentType = "application/pdf";
    private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public ExportWeatherHandler(IWeatherIntegrationService weatherService)
    {
        _weatherService = weatherService;
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
            Format.Pdf => GeneratePdf(dailyData, generatedAt),
            Format.Excel => GenerateExcel(dailyData, generatedAt),
            _ => throw new ValidationException("Invalid format. Must be Pdf or Excel.")
        };
    }
    private static ExportWeatherHandlerDto GeneratePdf(List<WeatherDayDto> weather, DateTime generatedAt)
    {
        PdfSharpCore.Fonts.GlobalFontSettings.FontResolver = Resources.Fonts.CustomFontResolver.Instance;

        using var doc = new PdfDocument();
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);
        var fontBody = new XFont("Arial", 12);

        double y = 40;

        gfx.DrawString("Weather Report", fontTitle, XBrushes.Black, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
        y += 40;
        gfx.DrawString($"Generated: {generatedAt:yyyy-MM-dd HH:mm:ss}", fontBody, XBrushes.Black, new XPoint(40, y));
        y += 30;

        gfx.DrawString("Date", fontBody, XBrushes.Black, new XPoint(40, y));
        gfx.DrawString("Max Temp (°C)", fontBody, XBrushes.Black, new XPoint(150, y));
        gfx.DrawString("Min Temp (°C)", fontBody, XBrushes.Black, new XPoint(280, y));
        gfx.DrawString("Wind Max (km/h)", fontBody, XBrushes.Black, new XPoint(420, y));
        gfx.DrawString("Precip (mm)", fontBody, XBrushes.Black, new XPoint(540, y));

        y += 20;

        foreach (var day in weather)
        {
            gfx.DrawString(day.Date, fontBody, XBrushes.Black, new XPoint(40, y));
            gfx.DrawString(day.TempMax.ToString("0.0"), fontBody, XBrushes.Black, new XPoint(150, y));
            gfx.DrawString(day.TempMin.ToString("0.0"), fontBody, XBrushes.Black, new XPoint(280, y));
            gfx.DrawString(day.WindSpeed.ToString("0.0"), fontBody, XBrushes.Black, new XPoint(420, y));
            gfx.DrawString(day.Precipitation.ToString("0.0"), fontBody, XBrushes.Black, new XPoint(540, y));
            y += 20;
        }

        using var stream = new MemoryStream();
        doc.Save(stream, false);

        return new ExportWeatherHandlerDto(
            stream.ToArray(),
            PdfContentType,
            $"WeatherReport_{generatedAt:yyyyMMddHHmmss}.pdf"
        );
    }

    private static ExportWeatherHandlerDto GenerateExcel(List<WeatherDayDto> weather, DateTime generatedAt)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Weather Report");

        sheet.Cell(1, 1).Value = "Date";
        sheet.Cell(1, 2).Value = "Max Temp (°C)";
        sheet.Cell(1, 3).Value = "Min Temp (°C)";
        sheet.Cell(1, 4).Value = "Wind Max (km/h)";
        sheet.Cell(1, 5).Value = "Precipitation (mm)";

        for (int i = 0; i < weather.Count; i++)
        {
            var d = weather[i];
            sheet.Cell(i + 2, 1).Value = d.Date;
            sheet.Cell(i + 2, 2).Value = d.TempMax;
            sheet.Cell(i + 2, 3).Value = d.TempMin;
            sheet.Cell(i + 2, 4).Value = d.WindSpeed;
            sheet.Cell(i + 2, 5).Value = d.Precipitation;
        }

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return new ExportWeatherHandlerDto(
            stream.ToArray(),
            ExcelContentType,
            $"WeatherReport_{generatedAt:yyyyMMddHHmmss}.xlsx"
        );
    }
}
public record WeatherDayDto(string Date, decimal TempMax, decimal TempMin, decimal WindSpeed, decimal Precipitation);
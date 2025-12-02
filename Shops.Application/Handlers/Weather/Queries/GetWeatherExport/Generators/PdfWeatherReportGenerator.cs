using Microsoft.Extensions.Options;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Configuration;
using Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Constants;

namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Generators;

public class PdfWeatherReportGenerator : IPdfWeatherReportGenerator
{
    private readonly PdfReportSettings _settings;

    public PdfWeatherReportGenerator(IOptions<PdfReportSettings> settings)
    {
        _settings = settings.Value;
    }

    public ExportWeatherHandlerDto Generate(List<WeatherDayDto> weatherData, DateTime generatedAt)
    {
        PdfSharpCore.Fonts.GlobalFontSettings.FontResolver = Resources.Fonts.CustomFontResolver.Instance;

        using var doc = new PdfDocument();
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);

        var fontTitle = new XFont(_settings.FontFamily, _settings.TitleFontSize, XFontStyle.Bold);
        var fontBody = new XFont(_settings.FontFamily, _settings.BodyFontSize);

        double y = PdfLayoutConstants.InitialYPosition;

        // Draw title
        gfx.DrawString(
            "Weather Report",
            fontTitle,
            XBrushes.Black,
            new XRect(0, y, page.Width, PdfLayoutConstants.TitleHeight),
            XStringFormats.TopCenter);
        y += PdfLayoutConstants.TitleSpacing;

        // Draw generation timestamp
        gfx.DrawString(
            $"Generated: {generatedAt.ToString(PdfLayoutConstants.GeneratedDateTimeFormat)}",
            fontBody,
            XBrushes.Black,
            new XPoint(PdfLayoutConstants.LeftMargin, y));
        y += PdfLayoutConstants.TimestampSpacing;

        // Draw table headers
        gfx.DrawString("Date", fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.DateColumnX, y));
        gfx.DrawString("Max Temp (°C)", fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.MaxTempColumnX, y));
        gfx.DrawString("Min Temp (°C)", fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.MinTempColumnX, y));
        gfx.DrawString("Wind Max (km/h)", fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.WindColumnX, y));
        gfx.DrawString("Precip (mm)", fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.PrecipitationColumnX, y));
        y += PdfLayoutConstants.HeaderSpacing;

        // Draw data rows
        foreach (var day in weatherData)
        {
            gfx.DrawString(day.Date, fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.DateColumnX, y));
            gfx.DrawString(day.TempMax.ToString(PdfLayoutConstants.DecimalFormat), fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.MaxTempColumnX, y));
            gfx.DrawString(day.TempMin.ToString(PdfLayoutConstants.DecimalFormat), fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.MinTempColumnX, y));
            gfx.DrawString(day.WindSpeed.ToString(PdfLayoutConstants.DecimalFormat), fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.WindColumnX, y));
            gfx.DrawString(day.Precipitation.ToString(PdfLayoutConstants.DecimalFormat), fontBody, XBrushes.Black, new XPoint(PdfLayoutConstants.PrecipitationColumnX, y));
            y += PdfLayoutConstants.RowSpacing;
        }

        using var stream = new MemoryStream();
        doc.Save(stream, false);

        return new ExportWeatherHandlerDto(
            stream.ToArray(),
            PdfLayoutConstants.ContentType,
            $"WeatherReport_{generatedAt.ToString(PdfLayoutConstants.FileNameDateFormat)}.pdf"
        );
    }
}

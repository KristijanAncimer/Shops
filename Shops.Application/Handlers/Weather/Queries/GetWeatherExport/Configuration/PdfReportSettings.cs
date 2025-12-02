namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Configuration;

public class PdfReportSettings
{
    public string FontFamily { get; set; } = "Arial";
    public double TitleFontSize { get; set; } = 18;
    public double BodyFontSize { get; set; } = 12;
}

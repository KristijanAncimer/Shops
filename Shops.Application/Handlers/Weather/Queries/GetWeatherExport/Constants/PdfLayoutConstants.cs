namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Constants;

public static class PdfLayoutConstants
{
    // Spacing and positioning
    public const double InitialYPosition = 40;
    public const double TitleHeight = 30;
    public const double TitleSpacing = 40;
    public const double TimestampSpacing = 30;
    public const double HeaderSpacing = 20;
    public const double RowSpacing = 20;

    // Column positions (X-axis)
    public const double LeftMargin = 40;
    public const double DateColumnX = 40;
    public const double MaxTempColumnX = 150;
    public const double MinTempColumnX = 280;
    public const double WindColumnX = 420;
    public const double PrecipitationColumnX = 540;

    // Formatting
    public const string DecimalFormat = "0.0";
    public const string GeneratedDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    public const string FileNameDateFormat = "yyyyMMddHHmmss";

    // Content type
    public const string ContentType = "application/pdf";
}

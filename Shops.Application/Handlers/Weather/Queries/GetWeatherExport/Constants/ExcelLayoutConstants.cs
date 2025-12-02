namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Constants;

public static class ExcelLayoutConstants
{
    // Row indices
    public const int HeaderRow = 1;
    public const int DataStartRow = 2;

    // Column indices
    public const int DateColumn = 1;
    public const int MaxTempColumn = 2;
    public const int MinTempColumn = 3;
    public const int WindColumn = 4;
    public const int PrecipitationColumn = 5;

    // Formatting
    public const string FileNameDateFormat = "yyyyMMddHHmmss";

    // Content type
    public const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    // Sheet name
    public const string SheetName = "Weather Report";
}

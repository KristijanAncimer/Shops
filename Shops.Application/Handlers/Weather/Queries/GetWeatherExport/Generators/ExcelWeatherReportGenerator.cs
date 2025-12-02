using ClosedXML.Excel;
using Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Constants;

namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport.Generators;

public class ExcelWeatherReportGenerator : IExcelWeatherReportGenerator
{
    public ExportWeatherHandlerDto Generate(List<WeatherDayDto> weatherData, DateTime generatedAt)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add(ExcelLayoutConstants.SheetName);

        // Set header row
        sheet.Cell(ExcelLayoutConstants.HeaderRow, ExcelLayoutConstants.DateColumn).Value = "Date";
        sheet.Cell(ExcelLayoutConstants.HeaderRow, ExcelLayoutConstants.MaxTempColumn).Value = "Max Temp (°C)";
        sheet.Cell(ExcelLayoutConstants.HeaderRow, ExcelLayoutConstants.MinTempColumn).Value = "Min Temp (°C)";
        sheet.Cell(ExcelLayoutConstants.HeaderRow, ExcelLayoutConstants.WindColumn).Value = "Wind Max (km/h)";
        sheet.Cell(ExcelLayoutConstants.HeaderRow, ExcelLayoutConstants.PrecipitationColumn).Value = "Precipitation (mm)";

        // Fill data rows
        for (int i = 0; i < weatherData.Count; i++)
        {
            var day = weatherData[i];
            int row = ExcelLayoutConstants.DataStartRow + i;

            sheet.Cell(row, ExcelLayoutConstants.DateColumn).Value = day.Date;
            sheet.Cell(row, ExcelLayoutConstants.MaxTempColumn).Value = day.TempMax;
            sheet.Cell(row, ExcelLayoutConstants.MinTempColumn).Value = day.TempMin;
            sheet.Cell(row, ExcelLayoutConstants.WindColumn).Value = day.WindSpeed;
            sheet.Cell(row, ExcelLayoutConstants.PrecipitationColumn).Value = day.Precipitation;
        }

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return new ExportWeatherHandlerDto(
            stream.ToArray(),
            ExcelLayoutConstants.ContentType,
            $"WeatherReport_{generatedAt.ToString(ExcelLayoutConstants.FileNameDateFormat)}.xlsx"
        );
    }
}

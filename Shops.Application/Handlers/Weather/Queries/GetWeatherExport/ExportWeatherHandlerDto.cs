namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport;

public class ExportWeatherHandlerDto
{
    public byte[] FileContent { get; }
    public string ContentType { get; }
    public string FileName { get; }

    public ExportWeatherHandlerDto(byte[] fileContent, string contentType, string fileName)
    {
        FileContent = fileContent;
        ContentType = contentType;
        FileName = fileName;
    }
}

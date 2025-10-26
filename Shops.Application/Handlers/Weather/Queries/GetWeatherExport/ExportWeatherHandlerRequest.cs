using MediatR;
using Shops.Application.Handlers.Shops.Queries.GetShops;
using System.Text.Json.Serialization;

namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport;

public class ExportWeatherHandlerRequest : IRequest<ExportWeatherHandlerDto>
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public int? DaysBack { get; set; }
    public Format Format { get; set; }
    public DateTime? GeneratedAt { get; set; }
    public int GetDaysBackOrDefault() => DaysBack ?? 7;

    public ExportWeatherHandlerRequest(decimal latitude, decimal longitude, int? daysBack, Format format, DateTime? generatedAt)
    {
        Latitude = latitude;
        Longitude = longitude;
        Format = format;
        DaysBack = daysBack;
        GeneratedAt = generatedAt;
    }
    public static ExportWeatherHandlerRequest Create(decimal latitude, decimal longitude, int? daysBack, Format format, DateTime? generatedAt)
        => new(latitude, longitude, daysBack, format, generatedAt);
}
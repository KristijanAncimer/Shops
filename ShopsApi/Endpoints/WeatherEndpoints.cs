using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shops.Application.Handlers.Weather.Queries.GetWeatherExport;

namespace ShopsApi.Endpoints;

public static class WeatherEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/Weather")
            .RequireAuthorization();

        mapGroup.MapGet("/export", ExportWeather)
            .RequireRateLimiting("fixed")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Exports weather data",
                Description = "Exports weather data as a PDF or Excel file"
            });

        return app;
    }

    private static async Task<IResult> ExportWeather(
        [FromServices] IMediator mediator,
        [FromQuery] decimal latitude,
        [FromQuery] decimal longitude,
        [FromQuery] int? daysBack,
        [FromQuery] Format format,
        [FromQuery] DateTime? generatedAt,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(ExportWeatherHandlerRequest.Create(latitude, longitude, daysBack, format, generatedAt), cancellationToken);
        return Results.File(result.FileContent, result.ContentType, result.FileName);
    }
}

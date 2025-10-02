using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shops.Application.Handlers.Shops.Commands.CreateShop;
using Shops.Application.Handlers.Shops.Commands.DeleteShop;
using Shops.Application.Handlers.Shops.Commands.UpdateShop;
using Shops.Application.Handlers.Shops.Queries.GetShopById;
using Shops.Application.Handlers.Shops.Queries.GetShops;

namespace ShopsApi.Endpoints;

public static class ShopEndpoints
{
    public static IEndpointRouteBuilder MapShopEndpoints(this IEndpointRouteBuilder app)
    {
        var mapGroup = app.MapGroup("/Shop")
            .RequireAuthorization();

        mapGroup.MapGet("/{id}", GetShopById)
            .RequireRateLimiting("fixed")
            .Produces<GetShopByIdHandlerDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Gets a shop",
                Description = "Gets a shop by its id"
            });

        mapGroup.MapGet("", GetAllShops)
            .RequireRateLimiting("fixed")
            .Produces<List<GetShopsHandlerDto>>(StatusCodes.Status200OK)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Gets all shops",
                Description = "Gets all shops with optional filtering by name"
            });

        mapGroup.MapPost("", CreateNewShop)
            .RequireRateLimiting("fixed")
            .Produces<CreateShopDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Creates a new shop",
                Description = "Creates a new shop"
            });

        mapGroup.MapPut("/{id}", UpdateShop)
            .RequireRateLimiting("fixed")
            .Produces<UpdateShopHandlerDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Updates an existing shop",
                Description = "Updates an existing shop"
            });

        mapGroup.MapDelete("/{id}", DeleteShop)
            .RequireRateLimiting("fixed")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Deletes a shop",
                Description = "Deletes a shop by its ID"
            });

        return app;
    }
    private static async Task<IResult> CreateNewShop(
    [FromServices] IMediator mediator,
    [FromBody] CreateShopHandlerRequest request,
    CancellationToken cancellationToken)
    {
        var result = await mediator.Send(CreateShopHandlerRequest.Create(request.Name), cancellationToken);
        return Results.Created($"/Shop/{result?.Data?.Id}", result);
    }
    private static async Task<IResult> DeleteShop(
    [FromServices] IMediator mediator,
    int id,
    CancellationToken cancellationToken)
    {
        await mediator.Send(DeleteShopHandlerRequest.Create(id), cancellationToken);
        return Results.NoContent();
    }
    private static async Task<IResult> GetShopById(
    [FromServices] IMediator mediator,
    int id,
    CancellationToken cancellationToken)
    {
        var shop = await mediator.Send(GetShopByIdHandlerRequest.Create(id), cancellationToken);
        return Results.Ok(shop);
    }
    private static async Task<IResult> GetAllShops(
    [FromServices] IMediator mediator,
    [FromQuery] string? filter,
    int? pageNumber,
    int? pageSize,
    CancellationToken cancellationToken)
    => Results.Ok(await mediator.Send(GetShopsHandlerRequest.Create(filter, pageNumber, pageSize), cancellationToken));
    private static async Task<IResult> UpdateShop(
    [FromServices] IMediator mediator,
    int id,
    [FromBody] UpdateShopHandlerRequest request,
    CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(UpdateShopHandlerRequest.Create(id, request.Name), cancellationToken);
        return Results.Ok(updated);
    }
}
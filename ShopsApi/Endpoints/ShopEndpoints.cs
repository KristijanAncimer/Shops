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
        var mapGroup = app.MapGroup("/Shop");

        mapGroup.MapGet("/{id}", GetShopById)
            .Produces<GetShopByIdHandlerDto>(StatusCodes.Status200OK)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Gets a shop",
            Description = "Gets a shop by its id"
        });

        mapGroup.MapGet("", GetAllShops)
        .Produces<List<GetShopsHandlerDto>>(StatusCodes.Status200OK)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Gets all shops",
            Description = "Gets all shops with optional filtering by name"
        });

        mapGroup.MapPost("", CreateNewShop)
        .Produces<string>(StatusCodes.Status200OK)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Creates a new shop",
            Description = "Creates a new shop"
        });

        mapGroup.MapPut("/{id}", UpdateShop)
        .Produces<UpdateShopHandlerDto>(StatusCodes.Status200OK)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Updates an existing shop",
            Description = "Updates an existing shop"
        });

        mapGroup.MapDelete("/{id}", DeleteShop)
            .Produces<string>(StatusCodes.Status200OK)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Deletes a shop",
                Description = "Deletes a shop by its ID"
            });

        return app;
    }
    private static async Task<IResult> CreateNewShop(
    [FromServices] IMediator mediator,
    string name,
    CancellationToken cancellationToken)
    => Results.Ok(await mediator.Send(CreateShopHandlerRequest.Create(name), cancellationToken));
    private static async Task<IResult> DeleteShop(
    [FromServices] IMediator mediator,
    int id,
    CancellationToken cancellationToken)
    => Results.Ok(await mediator.Send(DeleteShopHandlerRequest.Create(id), cancellationToken));
    private static async Task<IResult> GetShopById(
    [FromServices] IMediator mediator,
    int id,
    CancellationToken cancellationToken)
    => Results.Ok(await mediator.Send(GetShopByIdHandlerRequest.Create(id), cancellationToken));
    private static async Task<IResult> GetAllShops(
    [FromServices] IMediator mediator,
    [FromQuery] string? filter,
    CancellationToken cancellationToken)
    => Results.Ok(await mediator.Send(GetShopsHandlerRequest.Create(filter), cancellationToken));
    private static async Task<IResult> UpdateShop(
    [FromServices] IMediator mediator,
    int id,
    [FromBody] string name,
    CancellationToken cancellationToken)
    {
        var result = await mediator.Send(UpdateShopHandlerRequest.Create(id, name), cancellationToken);

        if (!result.IsSuccess)
            return Results.NotFound(result.Error);

        return Results.Ok(result.Data);
    }
}
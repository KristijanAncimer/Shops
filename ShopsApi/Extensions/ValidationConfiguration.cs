using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Shops.Application.Behaviors;
using Shops.Application.Handlers.Shops.Commands.CreateShop;

namespace ShopsApi.Extensions;

public static class ValidationConfiguration
{
    public static IServiceCollection AddValidationConfiguration(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateShopHandlerRequest>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

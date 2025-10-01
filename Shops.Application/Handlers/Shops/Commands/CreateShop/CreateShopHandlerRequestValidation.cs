using FluentValidation;

namespace Shops.Application.Handlers.Shops.Commands.CreateShop;

public class CreateShopHandlerRequestValidation : AbstractValidator<CreateShopHandlerRequest>
{
    public CreateShopHandlerRequestValidation()
    {
        RuleFor(x => x.Name)
            .MinimumLength(5)
            .WithMessage("Name of the shop must be at least 5 characters")
            .MaximumLength(50)
            .WithMessage("Name of the shop must not be more than 5 characters");
    }
}

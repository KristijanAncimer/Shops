using FluentValidation;

namespace Shops.Application.Handlers.Shops.Commands.UpdateShop;

public class UpdateShopHandlerRequestValidation : AbstractValidator<UpdateShopHandlerRequest>
{
    public UpdateShopHandlerRequestValidation()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Shop ID must be greater than 0.");

        RuleFor(x => x.Name)
            .MinimumLength(5)
            .WithMessage("Name of the shop must be at least 5 characters")
            .MaximumLength(50)
            .WithMessage("Name of the shop must not be more than 5 characters");

    }
}

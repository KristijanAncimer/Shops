using FluentValidation;

namespace Shops.Application.Handlers.Shops.Commands.CreateShop;

public class CreateShopHandlerRequestValidation : AbstractValidator<CreateShopHandlerRequest>
{
    public CreateShopHandlerRequestValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name of the shop must not be empty.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name of the shop must not contain only whitespace.")
            .MinimumLength(5)
            .WithMessage("Name of the shop must be at least 5 characters.")
            .MaximumLength(50)
            .WithMessage("Name of the shop must not be more than 50 characters.")
            .Matches("^[A-Za-z ]+$")
            .WithMessage("Name of the shop can only contain letters and spaces.");
    }
}

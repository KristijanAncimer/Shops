using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shops.Infrastructure.Persistance;

namespace Shops.Application.Handlers.Shops.Commands.DeleteShop;

public class DeleteShopHandlerRequestValidation : AbstractValidator<DeleteShopHandlerRequest>
{
    public DeleteShopHandlerRequestValidation()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Shop ID must be greater than 0.");
    }
}

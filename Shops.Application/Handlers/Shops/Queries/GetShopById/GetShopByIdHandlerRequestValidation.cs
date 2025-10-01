using FluentValidation;

namespace Shops.Application.Handlers.Shops.Queries.GetShopById;

public class GetShopByIdHandlerRequestValidation : AbstractValidator<GetShopByIdHandlerRequest>
{
    public GetShopByIdHandlerRequestValidation()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Shop ID must be greater than 0.");
    }
}

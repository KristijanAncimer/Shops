using FluentValidation;

namespace Shops.Application.Handlers.Shops.Queries.GetShops;

public class GetShopsHandlerRequestValidator : AbstractValidator<GetShopsHandlerRequest>
{
    public GetShopsHandlerRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");
    }
}

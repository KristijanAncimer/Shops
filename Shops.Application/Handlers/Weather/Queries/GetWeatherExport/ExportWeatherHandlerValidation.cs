using FluentValidation;
using Shops.Application.Handlers.Shops.Commands.UpdateShop;

namespace Shops.Application.Handlers.Weather.Queries.GetWeatherExport;

public class ExportWeatherHandlerValidation : AbstractValidator<ExportWeatherHandlerRequest>
{
    public ExportWeatherHandlerValidation()
    {
        RuleFor(x => x.DaysBack)
            .Cascade(CascadeMode.Stop)
            .Must(x => x == null || (x >= 0 && x <= 30))
            .WithMessage("DaysBack must be between 0 and 30 if specified.");
    }
}

using FluentValidation;

namespace Application.UseCases.Cities.Commands.Create;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Name).NotEmpty();
        
        RuleFor(x => x.Country).NotNull();
        RuleFor(x => x.Country).NotEmpty();

        RuleFor(x => x.SubCountry).NotNull();
        RuleFor(x => x.SubCountry).NotEmpty();
        
        RuleFor(x => x.GeonameId).NotNull();
        RuleFor(x => x.GeonameId).NotEmpty();
    }
}
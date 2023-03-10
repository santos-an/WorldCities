using FluentValidation;

namespace Application.UseCases.Cities.Queries.GetByCountry;

public class GetCitiesWithCountryQueryValidator : AbstractValidator<GetCitiesWithCountryQuery>
{
    public GetCitiesWithCountryQueryValidator()
    {
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.Country).NotNull();

        RuleFor(x => x.Country).Length(3, 20);
    }
}
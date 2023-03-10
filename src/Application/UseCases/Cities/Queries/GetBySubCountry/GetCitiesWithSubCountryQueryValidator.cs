using FluentValidation;

namespace Application.UseCases.Cities.Queries.GetBySubCountry;

public class GetCitiesWithSubCountryQueryValidator : AbstractValidator<GetCitiesWithSubCountryQuery>
{
    public GetCitiesWithSubCountryQueryValidator()
    {
        RuleFor(x => x.SubCountry).NotEmpty();
        RuleFor(x => x.SubCountry).NotNull();

        RuleFor(x => x.SubCountry).Length(3, 20);
    }
}
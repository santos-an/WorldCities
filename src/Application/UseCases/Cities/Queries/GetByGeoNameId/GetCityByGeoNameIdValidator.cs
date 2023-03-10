using FluentValidation;

namespace Application.UseCases.Cities.Queries.GetByGeoNameId;

public class GetCityByGeoNameIdValidator : AbstractValidator<GetCityByGeoNameId>
{
    public GetCityByGeoNameIdValidator()
    {
        RuleFor(x => x.GeonameId).NotNull();
        RuleFor(x => x.GeonameId).NotEmpty();

        RuleFor(x => x.GeonameId).Length(4, 10);
    }
}
using FluentValidation;

namespace Application.UseCases.Cities.Queries.GetByName;

public class GetCitiesWithNameQueryValidator : AbstractValidator<GetCitiesWithNameQuery>
{
    public GetCitiesWithNameQueryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).NotNull();

        RuleFor(x => x.Name).MinimumLength(3);
    }
}
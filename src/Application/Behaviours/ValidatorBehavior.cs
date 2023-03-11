using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = Application.Exceptions.ValidationException;

namespace Application.Behaviours;

public sealed class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var errors = GetValidationErrors(context);
        var errorsDictionary = ToDictionary(errors);

        if (errorsDictionary.Any())
            throw new ValidationException(errorsDictionary);

        return await next();
    }

    private IEnumerable<ValidationFailure> GetValidationErrors(IValidationContext context)
    {
        return _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x is not null);
    }

    private static Dictionary<string, string[]> ToDictionary(IEnumerable<ValidationFailure> errors)
    {
        return errors
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);
    }
}
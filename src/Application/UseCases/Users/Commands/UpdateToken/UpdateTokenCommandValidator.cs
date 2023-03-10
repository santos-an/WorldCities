using FluentValidation;

namespace Application.UseCases.Users.Commands.UpdateToken;

public class UpdateTokenCommandValidator : AbstractValidator<UpdateTokenCommand>
{
    public UpdateTokenCommandValidator()
    {
        RuleFor(x => x.AccessToken).NotNull();
        RuleFor(x => x.AccessToken).NotEmpty();
        
        RuleFor(x => x.RefreshToken).NotNull();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
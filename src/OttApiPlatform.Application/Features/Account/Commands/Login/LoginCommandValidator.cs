namespace OttApiPlatform.Application.Features.Account.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    #region Public Constructors

    public LoginCommandValidator()
    {
        RuleFor(v => v.Email).Cascade(CascadeMode.Stop)
                             .NotEmpty()
                             .WithMessage(Resource.Username_is_required)
                             .EmailAddress()
                             .WithMessage(v => string.Format(Resource.Username_is_invalid, v.Email));

        RuleFor(v => v.Password).Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                .WithMessage(Resource.Password_is_required);
    }

    #endregion Public Constructors
}
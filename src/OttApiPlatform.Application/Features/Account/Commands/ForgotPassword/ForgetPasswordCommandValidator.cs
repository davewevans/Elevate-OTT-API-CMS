namespace OttApiPlatform.Application.Features.Account.Commands.ForgotPassword;

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordCommand>
{
    #region Public Constructors

    public ForgetPasswordValidator()
    {
        RuleFor(v => v.Email).Cascade(CascadeMode.Stop)
                             .NotEmpty()
                             .WithMessage(Resource.Username_is_required)
                             .EmailAddress()
                             .WithMessage(v => string.Format(Resource.Username_is_invalid, v.Email))
                             .MaximumLength(100)
                             .WithMessage(Resource.Username_must_not_exceed_200_characters)
                             .MinimumLength(6)
                             .WithMessage(Resource.Username_must_be_at_least_6_characters);
    }

    #endregion Public Constructors
}
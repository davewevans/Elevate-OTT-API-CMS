namespace OttApiPlatform.Application.Features.Account.Commands.LoginWith2fa;

public class LoginWith2FaCommandValidator : AbstractValidator<LoginWith2FaCommand>
{
    #region Public Constructors

    public LoginWith2FaCommandValidator()
    {
        RuleFor(v => v.TwoFactorCode).Cascade(CascadeMode.Stop)
                                     .NotEmpty()
                                     .WithMessage(Resource.Two_factor_authentication_code_is_required)
                                     .MaximumLength(7)
                                     .WithMessage(Resource.Two_factor_authentication_code_must_not_exceed_7_characters)
                                     .MinimumLength(6)
                                     .WithMessage(Resource.Two_factor_authentication_code_must_be_at_least_6_character_long);
    }

    #endregion Public Constructors
}
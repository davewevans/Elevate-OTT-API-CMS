namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.LoginWithRecoveryCode;

public class LoginWithRecoveryCodeCommandValidator : AbstractValidator<LoginWithRecoveryCodeCommand>
{
    #region Public Constructors

    public LoginWithRecoveryCodeCommandValidator()
    {
        RuleFor(v => v.RecoveryCode).Cascade(CascadeMode.Stop)
                                    .NotEmpty()
                                    .WithMessage(Resource.Recovery_Code_is_required);
    }

    #endregion Public Constructors
}
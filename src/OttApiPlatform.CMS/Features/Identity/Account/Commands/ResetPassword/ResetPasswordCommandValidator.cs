namespace OttApiPlatform.CMS.Features.Identity.Account.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    #region Public Constructors

    public ResetPasswordCommandValidator()
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

        RuleFor(v => v.Password).Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                .WithMessage(Resource.Password_is_required)
                                .MaximumLength(100)
                                .WithMessage(Resource.Password_must_not_exceed_200_characters)
                                .MinimumLength(6)
                                .WithMessage(Resource.Password_must_be_at_least_6_characters);

        RuleFor(v => v.ConfirmPassword).Cascade(CascadeMode.Stop)
                                       .NotEmpty()
                                       .WithMessage(Resource.Confirm_password_is_required)
                                       .Equal(register => register.ConfirmPassword)
                                       .WithMessage(Resource.The_password_and_confirmation_password_do_not_match);
    }

    #endregion Public Constructors
}
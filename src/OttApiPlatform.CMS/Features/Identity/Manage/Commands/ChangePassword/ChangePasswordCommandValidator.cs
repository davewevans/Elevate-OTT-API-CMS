namespace OttApiPlatform.CMS.Features.Identity.Manage.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    #region Public Constructors

    public ChangePasswordCommandValidator()
    {
        RuleFor(v => v.OldPassword).Cascade(CascadeMode.Stop)
                                   .NotEmpty()
                                   .WithMessage(Resource.Old_password_is_required);

        RuleFor(v => v.NewPassword).Cascade(CascadeMode.Stop)
                                   .NotEmpty()
                                   .WithMessage(Resource.New_password_is_required)
                                   .MaximumLength(100)
                                   .WithMessage(Resource.New_password_must_not_exceed_200_characters)
                                   .MinimumLength(6)
                                   .WithMessage(Resource.New_password_must_be_at_least_6_characters);

        RuleFor(v => v.ConfirmPassword).Cascade(CascadeMode.Stop)
                                       .NotEmpty()
                                       .WithMessage(Resource.Confirm_password_is_required)
                                       .Equal(v => v.NewPassword)
                                       .WithMessage(Resource.The_password_and_confirmation_password_do_not_match);
    }

    #endregion Public Constructors
}
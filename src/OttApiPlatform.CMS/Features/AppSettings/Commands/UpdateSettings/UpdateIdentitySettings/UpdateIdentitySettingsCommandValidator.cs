namespace OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class IdentitySettingsForEditValidator : AbstractValidator<IdentitySettingsForEdit>
{
    #region Public Constructors

    public IdentitySettingsForEditValidator()
    {
        RuleFor(v => v.UserSettingsForEdit.AllowedUserNameCharacters).Cascade(CascadeMode.Stop)
                                                                     .NotEmpty()
                                                                     .WithMessage(Resource.Allowed_username_characters_are_required);

        RuleFor(v => v.LockoutSettingsForEdit.DefaultLockoutTimeSpan).Cascade(CascadeMode.Stop)
                                                                     .NotEmpty()
                                                                     .WithMessage(Resource.Default_lockout_time_Span_is_required);

        RuleFor(v => v.LockoutSettingsForEdit.MaxFailedAccessAttempts).Cascade(CascadeMode.Stop)
                                                                      .NotEmpty()
                                                                      .WithMessage(Resource.Max_failed_access_attempt_is_required);

        RuleFor(v => v.PasswordSettingsForEdit.RequiredLength).Cascade(CascadeMode.Stop)
                                                              .NotEmpty()
                                                              .WithMessage(Resource.Required_length_is_required);

        RuleFor(v => v.PasswordSettingsForEdit.RequiredUniqueChars).Cascade(CascadeMode.Stop)
                                                                   .NotEmpty()
                                                                   .WithMessage(Resource.Required_unique_characters_is_required);
    }

    #endregion Public Constructors
}
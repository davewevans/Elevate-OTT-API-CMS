namespace OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateTokenSettings;

public class TokenSettingsForEditValidator : AbstractValidator<TokenSettingsForEdit>
{
    #region Public Constructors

    public TokenSettingsForEditValidator()
    {
        RuleFor(v => v.AccessTokenTimeSpan).Cascade(CascadeMode.Stop)
                                           .NotEmpty()
                                           .WithMessage(Resource.Access_token_timespan_is_required);

        RuleFor(v => v.RefreshTokenTimeSpan).Cascade(CascadeMode.Stop)
                                            .NotEmpty()
                                            .WithMessage(Resource.Refresh_token_timespan_is_required)
                                            .GreaterThan(v => v.AccessTokenTimeSpan)
                                            .When(v => v.AccessTokenTimeSpan != null)
                                            .WithMessage(Resource.Refresh_token_timespan_must_be_greater_than_access_token_expiry_time);
    }

    #endregion Public Constructors
}
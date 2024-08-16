namespace OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateTokenSettings;

public class UpdateTokenSettingsCommandValidator : AbstractValidator<UpdateTokenSettingsCommand>
{
    #region Public Constructors

    public UpdateTokenSettingsCommandValidator()
    {
        RuleFor(v => v.AccessTokenTimeSpan).Cascade(CascadeMode.Stop)
                                                         .NotEmpty()
                                                         .WithMessage(Resource.Access_token_timespan_is_required);

        RuleFor(v => v.RefreshTokenTimeSpan).Cascade(CascadeMode.Stop)
                                                          .NotEmpty()
                                                          .WithMessage(Resource.Refresh_token_timespan_cannot_be_null)
                                                          .GreaterThan(v => v.AccessTokenTimeSpan)
                                                          .When(v => v.AccessTokenTimeSpan != null)
                                                          .WithMessage(Resource.Refresh_token_timespan_must_be_greater_than_access_token_expiry_time);
    }

    #endregion Public Constructors
}
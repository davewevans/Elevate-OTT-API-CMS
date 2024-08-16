namespace OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class IdentitySettingsResponse
{
    #region Public Properties

    public Guid UserSettingsId { get; set; }
    public Guid PasswordSettingsId { get; set; }
    public Guid LockoutSettingsId { get; set; }
    public Guid SignInSettingsId { get; set; }
    public string SuccessMessage { get; set; }

    #endregion Public Properties
}
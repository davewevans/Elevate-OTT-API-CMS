namespace OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class IdentitySettingsResponse
{
    #region Public Properties

    public string UserSettingsId { get; set; }
    public string PasswordSettingsId { get; set; }
    public string LockoutSettingsId { get; set; }
    public string SignInSettingsId { get; set; }
    public string SuccessMessage { get; set; }

    #endregion Public Properties
}
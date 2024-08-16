namespace OttApiPlatform.CMS.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class UpdateIdentitySettingsCommand
{
    #region Public Properties

    public UserSettingsCommand UserSettingsCommand { get; set; }
    public PasswordSettingsCommand PasswordSettingsCommand { get; set; }
    public LockoutSettingsCommand LockoutSettingsCommand { get; set; }
    public SignInSettingsCommand SignInSettingsCommand { get; set; }

    #endregion Public Properties
}
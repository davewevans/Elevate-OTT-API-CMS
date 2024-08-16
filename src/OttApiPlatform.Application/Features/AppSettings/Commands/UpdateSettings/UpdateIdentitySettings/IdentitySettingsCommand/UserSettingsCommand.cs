namespace OttApiPlatform.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.IdentitySettingsCommand;

public class UserSettingsCommand
{
    #region Public Properties

    public string Id { get; set; }
    public string AllowedUserNameCharacters { get; set; }
    public bool NewUsersActiveByDefault { get; set; }

    #endregion Public Properties
}
namespace OttApiPlatform.CMS.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class UserSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }
    public string AllowedUserNameCharacters { get; set; }
    public bool NewUsersActiveByDefault { get; set; }

    #endregion Public Properties

    #region Public Methods

    public UserSettingsCommand MapToCommand()
    {
        return new UserSettingsCommand
        {
            Id = Id,
            AllowedUserNameCharacters = AllowedUserNameCharacters,
            NewUsersActiveByDefault = NewUsersActiveByDefault
        };
    }

    #endregion Public Methods
}
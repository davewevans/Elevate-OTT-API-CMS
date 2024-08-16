namespace OttApiPlatform.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class UserSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }
    public string AllowedUserNameCharacters { get; set; }
    public bool NewUsersActiveByDefault { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static UserSettingsForEdit MapFromEntity(UserSettings userSettings)
    {
        return new UserSettingsForEdit
        {
            Id = userSettings.Id.ToString(),
            AllowedUserNameCharacters = userSettings.AllowedUserNameCharacters,
            NewUsersActiveByDefault = userSettings.NewUsersActiveByDefault
        };
    }

    #endregion Public Methods
}
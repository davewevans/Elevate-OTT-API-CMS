namespace OttApiPlatform.Application.Common.Models.ApplicationOptions.ApplicationIdentityOptions;

/// <summary>
/// Represents the options related to the application user settings.
/// </summary>
public class AppUserOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppUserOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the characters that are allowed in user names.
    /// </summary>
    public string AllowedUserNameCharacters { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether new users are active by default.
    /// </summary>
    public bool NewUsersActiveByDefault { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Maps the options to an instance of the <see cref="UserSettings"/> class.
    /// </summary>
    /// <returns>An instance of the <see cref="UserSettings"/> class.</returns>
    public UserSettings MapToEntity()
    {
        return new()
        {
            AllowedUserNameCharacters = AllowedUserNameCharacters,
            NewUsersActiveByDefault = NewUsersActiveByDefault
        };
    }

    #endregion Public Methods
}
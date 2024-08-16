namespace OttApiPlatform.Domain.Entities.Settings.IdentitySettings;

/// <summary>
/// Represents the user settings for user accounts.
/// </summary>
public class UserSettings : ISettingsSchema, IMayHaveTenant
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the ID of the user settings.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the characters allowed in a user name.
    /// </summary>
    public string AllowedUserNameCharacters { get; set; }

    /// <summary>
    /// Gets or sets whether new users are active by default.
    /// </summary>
    public bool NewUsersActiveByDefault { get; set; }

    public Guid? TenantId { get; set; }

    #endregion Public Properties
}

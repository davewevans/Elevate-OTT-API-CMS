namespace OttApiPlatform.Application.Common.Models.ApplicationOptions.ApplicationIdentityOptions;

/// <summary>
/// Represents options for lockout settings in the application.
/// </summary>
public class AppLockoutOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppLockoutOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating whether lockout is allowed for new users.
    /// </summary>
    public bool AllowedForNewUsers { get; set; }

    /// <summary>
    /// Gets or sets the maximum failed access attempts before a user is locked out.
    /// </summary>
    public int MaxFailedAccessAttempts { get; set; }

    /// <summary>
    /// Gets or sets the default lockout duration in seconds.
    /// </summary>
    public int DefaultLockoutTimeSpan { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Maps the AppLockoutOptions object to a LockoutSettings object.
    /// </summary>
    /// <returns>A LockoutSettings object.</returns>
    public LockoutSettings MapToEntity()
    {
        return new()
        {
            AllowedForNewUsers = AllowedForNewUsers,
            MaxFailedAccessAttempts = MaxFailedAccessAttempts,
            DefaultLockoutTimeSpan = DefaultLockoutTimeSpan
        };
    }

    #endregion Public Methods
}
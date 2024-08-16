namespace OttApiPlatform.Application.Common.Models.ApplicationOptions.ApplicationIdentityOptions;

public class AppIdentityOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppIdentityOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// The user options used by the identity system.
    /// </summary>
    public AppUserOptions AppUserOptions { get; set; }

    /// <summary>
    /// The password options used by the identity system.
    /// </summary>
    public AppPasswordOptions AppPasswordOptions { get; set; }

    /// <summary>
    /// The lockout options used by the identity system.
    /// </summary>
    public AppLockoutOptions AppLockoutOptions { get; set; }

    /// <summary>
    /// The sign-in options used by the identity system.
    /// </summary>
    public AppSignInOptions AppSignInOptions { get; set; }

    #endregion Public Properties
}
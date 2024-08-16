namespace OttApiPlatform.Application.Common.Models.ApplicationOptions.ApplicationIdentityOptions;

/// <summary>
/// Represents options for configuring sign-in settings.
/// </summary>
public class AppSignInOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppSignInOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating whether a confirmed account is required for sign-in.
    /// </summary>
    public bool RequireConfirmedAccount { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Maps AppSignInOptions to SignInSettings entity.
    /// </summary>
    /// <returns>SignInSettings entity.</returns>
    public SignInSettings MapToEntity()
    {
        return new()
        {
            RequireConfirmedAccount = RequireConfirmedAccount
        };
    }

    #endregion Public Methods
}
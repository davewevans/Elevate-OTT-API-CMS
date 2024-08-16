namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents options for app token settings, including access and refresh token's Unit of Time and TimeSpans.
/// </summary>
public class AppTokenOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppTokenOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the Unit of Time for the access token.
    /// </summary>
    public int AccessTokenUoT { get; set; }

    /// <summary>
    /// Gets or sets the TimeSpan for the access token.
    /// </summary>
    public double AccessTokenTimeSpan { get; set; }

    /// <summary>
    /// Gets or sets the Unit of Time for the refresh token.
    /// </summary>
    public int RefreshTokenUoT { get; set; }

    /// <summary>
    /// Gets or sets the TimeSpan for the refresh token.
    /// </summary>
    public double RefreshTokenTimeSpan { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Maps the AppTokenOptions to a TokenSettings entity.
    /// </summary>
    /// <returns>The TokenSettings entity</returns>
    public TokenSettings MapToEntity()
    {
        return new()
        {
            AccessTokenUoT = AccessTokenUoT,
            AccessTokenTimeSpan = AccessTokenTimeSpan,
            RefreshTokenTimeSpan = RefreshTokenTimeSpan,
            RefreshTokenUoT = RefreshTokenUoT,
        };
    }

    #endregion Public Methods
}
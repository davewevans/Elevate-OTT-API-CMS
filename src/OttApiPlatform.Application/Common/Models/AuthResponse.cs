namespace OttApiPlatform.Application.Common.Models;

/// <summary>
/// Represents the authentication response containing access and refresh tokens.
/// </summary>
public class AuthResponse
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the access token generated during authentication.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the refresh token generated during authentication.
    /// </summary>
    public string RefreshToken { get; set; }

    #endregion Public Properties
}
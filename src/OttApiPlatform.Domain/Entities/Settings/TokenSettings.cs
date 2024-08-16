namespace OttApiPlatform.Domain.Entities.Settings;

/// <summary>
/// Represents the settings for authentication tokens.
/// </summary>
public class TokenSettings : ISettingsSchema, IMayHaveTenant
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unit of time for the access token.
    /// </summary>
    public int AccessTokenUoT { get; set; }

    /// <summary>
    /// Gets or sets the time span for the access token, in the unit of time specified by <see cref="AccessTokenUoT"/>.
    /// </summary>
    public double? AccessTokenTimeSpan { get; set; }

    /// <summary>
    /// Gets or sets the unit of time for the refresh token.
    /// </summary>
    public int RefreshTokenUoT { get; set; }

    /// <summary>
    /// Gets or sets the time span for the refresh token, in the unit of time specified by <see cref="RefreshTokenUoT"/>.
    /// </summary>
    public double? RefreshTokenTimeSpan { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the tenant associated with the token settings.
    /// </summary>
    public Guid? TenantId { get; set; }

    #endregion Public Properties
}
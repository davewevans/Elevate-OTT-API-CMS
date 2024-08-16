namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents options for JWT configuration.
/// </summary>
public class JwtOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "jwt";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the issuer that should be used when creating JWT tokens.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the security key that should be used when creating and validating JWT tokens.
    /// </summary>
    public string SecurityKey { get; set; }

    /// <summary>
    /// Gets or sets the audience that should be used when creating JWT tokens.
    /// </summary>
    public string Audience { get; set; }

    #endregion Public Properties
}
namespace OttApiPlatform.Application.Common.Models.ApplicationOptions.ApplicationIdentityOptions;

/// <summary>
/// Represents options for configuring password policy.
/// </summary>
public class AppPasswordOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppPasswordOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the minimum length a password must be.
    /// </summary>
    public int RequiredLength { get; set; }

    /// <summary>
    /// Gets or sets the number of unique characters a password must contain.
    /// </summary>
    public int RequiredUniqueChars { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a password must contain at least one.
    /// non-alphanumeric character.
    /// </summary>
    public bool RequireNonAlphanumeric { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a password must contain at least one lowercase letter.
    /// </summary>
    public bool RequireLowercase { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a password must contain at least one uppercase letter.
    /// </summary>
    public bool RequireUppercase { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a password must contain at least one digit ('0'-'9').
    /// </summary>
    public bool RequireDigit { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Maps the current instance to a new <see cref="PasswordSettings"/> instance.
    /// </summary>
    /// <returns>The new <see cref="PasswordSettings"/> instance.</returns>
    public PasswordSettings MapToEntity()
    {
        return new()
        {
            RequiredLength = RequiredLength,
            RequiredUniqueChars = RequiredUniqueChars,
            RequireNonAlphanumeric = RequireNonAlphanumeric,
            RequireLowercase = RequireLowercase,
            RequireUppercase = RequireUppercase,
            RequireDigit = RequireDigit
        };
    }

    #endregion Public Methods
}
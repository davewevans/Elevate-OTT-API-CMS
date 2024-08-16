namespace OttApiPlatform.Domain.Entities.Settings.IdentitySettings;

/// <summary>
/// Represents the password settings for user accounts.
/// </summary>
public class PasswordSettings : ISettingsSchema, IMayHaveTenant
{
    /// <summary>
    /// Gets or sets the ID of the password settings.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the minimum required length of a password.
    /// </summary>
    public int RequiredLength { get; set; }

    /// <summary>
    /// Gets or sets the minimum required number of unique characters in a password.
    /// </summary>
    public int RequiredUniqueChars { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a password requires a non-alphanumeric character.
    /// </summary>
    public bool RequireNonAlphanumeric { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a password requires a lowercase letter.
    /// </summary>
    public bool RequireLowercase { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a password requires an uppercase letter.
    /// </summary>
    public bool RequireUppercase { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a password requires a digit.
    /// </summary>
    public bool RequireDigit { get; set; }

    public Guid? TenantId { get; set; }
}

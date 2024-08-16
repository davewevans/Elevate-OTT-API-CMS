namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents options for configuring app tenant functionality.
/// </summary>
public class AppTenantOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppTenantOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the tenant mode.
    /// </summary>
    public int TenantMode { get; set; }

    /// <summary>
    /// Gets or sets the data isolation strategy.
    /// </summary>
    public int DataIsolationStrategy { get; set; }

    #endregion Public Properties
}
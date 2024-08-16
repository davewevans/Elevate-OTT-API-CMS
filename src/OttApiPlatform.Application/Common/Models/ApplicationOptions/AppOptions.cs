namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

public class AppOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Identity options for the application.
    /// </summary>
    public AppIdentityOptions AppIdentityOptions { get; set; }

    /// <summary>
    /// Token options for the application.
    /// </summary>
    public AppTokenOptions AppTokenOptions { get; set; }

    /// <summary>
    /// File storage options for the application.
    /// </summary>
    public AppFileStorageOptions AppFileStorageOptions { get; set; }

    /// <summary>
    /// Tenant options for the application.
    /// </summary>
    public AppTenantOptions AppTenantOptions { get; set; }

    #endregion Public Properties
}
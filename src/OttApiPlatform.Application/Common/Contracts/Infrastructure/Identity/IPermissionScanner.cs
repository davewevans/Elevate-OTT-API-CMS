namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Identity;

/// <summary>
/// Generates application built-in permissions.
/// </summary>
public interface IPermissionScanner
{
    #region Public Methods

    /// <summary>
    /// Scans the built-in API endpoints to generate system permissions.
    /// </summary>
    Task ScanAndSeedBuiltInPermissions();

    #endregion Public Methods
}
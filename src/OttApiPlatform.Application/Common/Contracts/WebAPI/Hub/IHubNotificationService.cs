namespace OttApiPlatform.Application.Common.Contracts.WebAPI.Hub;

/// <summary>
/// Sends SignalR notifications to the client application.
/// </summary>
public interface IHubNotificationService
{
    #region Public Methods

    /// <summary>
    /// Notifies the report issuer of a report creation, deletion or update.
    /// </summary>
    /// <param name="userNameIdentifier">The unique identifier of the user to be notified.</param>
    /// <param name="fileMetaData">The metadata of the created, deleted or updated file.</param>
    /// <param name="status">The status of the report (created, deleted, or updated).</param>
    Task NotifyReportIssuer(string userNameIdentifier, FileMetaData fileMetaData, ReportStatus status);

    /// <summary>
    /// Notifies all report viewer clients to refresh their UI after a report is created, deleted or updated.
    /// </summary>
    /// <param name="userNameIdentifier">The unique identifier of the user to be notified.</param>
    Task RefreshReportsViewer(string userNameIdentifier);

    #endregion Public Methods
}
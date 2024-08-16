namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// Exports application-specific reports using background job.
/// </summary>
public interface IBackgroundReportingService
{
    #region Public Methods

    /// <summary>
    /// Initiates the generation of a report for the specified applicants query and report ID.
    /// </summary>
    /// <param name="request">The request containing the applicants query.</param>
    /// <param name="reportId">The report ID.</param>
    /// <param name="userNameIdentifier">The user name identifier.</param>
    /// <param name="tenantId">The ID of the tenant.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InitiateApplicantsReport(ExportApplicantsQuery request, Guid reportId, string userNameIdentifier, Guid? tenantId = null);

    /// <summary>
    /// Exports a list of applicants as a PDF file in the background and returns the file metadata.
    /// </summary>
    /// <param name="request">The request containing the applicants query.</param>
    /// <param name="reportId">The report ID.</param>
    /// <param name="baseUrl">The host URL.</param>
    /// <param name="userNameIdentifier">The user name identifier.</param>
    /// <param name="tenantId">The ID of the tenant.</param>
    /// <returns>The file metadata of the exported PDF file.</returns>
    Task<FileMetaData> ExportApplicantsAsPdfInBackground(ExportApplicantsQuery request, Guid reportId, string baseUrl, string userNameIdentifier, Guid? tenantId = null);

    #endregion Public Methods
}
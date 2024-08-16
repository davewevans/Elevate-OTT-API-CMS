namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// Exports application-specific reports on demand.
/// </summary>
public interface IOnDemandReportingService
{
    #region Public Methods

    /// <summary>
    /// Exports a list of applicants on demand as a PDF file and returns the file metadata.
    /// </summary>
    /// <param name="applicantsResponse">The response containing the applicants to be exported.</param>
    /// <param name="baseUrl">The host URL.</param>
    /// <param name="issuerName">The name of the issuer.</param>
    /// <returns>The file metadata of the exported PDF file.</returns>
    Task<ExportApplicantsResponse> ExportApplicantToPdfOnDemand(ApplicantsResponse applicantsResponse, string baseUrl, string issuerName);

    #endregion Public Methods
}
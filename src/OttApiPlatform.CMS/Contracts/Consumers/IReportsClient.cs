namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for managing reports.
/// </summary>
public interface IReportsClient
{
    #region Public Methods

    /// <summary>
    /// Retrieves a report for editing by ID.
    /// </summary>
    /// <param name="request">The query specifying the report ID.</param>
    /// <returns>A <see cref="ReportForEdit"/>.</returns>
    Task<ApiResponseWrapper<ReportForEdit>> GetReport(GetReportForEditQuery request);

    /// <summary>
    /// Retrieves a list of reports.
    /// </summary>
    /// <param name="request">The query specifying the search criteria.</param>
    /// <returns>A <see cref="ReportsResponse"/>.</returns>
    Task<ApiResponseWrapper<ReportsResponse>> GetReports(GetReportsQuery request);

    #endregion Public Methods
}
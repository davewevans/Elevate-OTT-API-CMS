namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// Generates HTML application reports.
/// </summary>
public interface IHtmlReportBuilderService
{
    #region Public Methods

    /// <summary>
    /// Gets the path of a file in the specified container with the specified file name.
    /// </summary>
    /// <param name="containerName">The name of the container.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <returns>The path of the file.</returns>
    string GetPath(string containerName, string fileName);

    /// <summary>
    /// Generates a PDF report containing the specified list of applicant items using the specified.
    /// HTML template.
    /// </summary>
    /// <param name="applicantItems">The list of applicant items to include in the report.</param>
    /// <param name="baseUrl">The base URL of the host.</param>
    /// <returns>A file metadata object containing information about the generated report.</returns>
    Task<FileMetaData> GenerateApplicantsPdfFromHtml
        (IReadOnlyList<ApplicantItem> applicantItems,
         string baseUrl);

    #endregion Public Methods
}
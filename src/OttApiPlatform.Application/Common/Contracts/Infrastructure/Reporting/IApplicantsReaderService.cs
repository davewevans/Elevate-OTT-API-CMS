namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// Represents a service for reading applicants.
/// </summary>
public interface IApplicantsReaderService
{
    #region Public Methods

    /// <summary>
    /// Retrieves a list of applicants based on the specified query.
    /// </summary>
    /// <param name="request">The query specifying the applicants to retrieve.</param>
    /// <returns>An envelope containing the applicants response.</returns>
    Task<Envelope<ApplicantsResponse>> GetApplicants(GetApplicantsQuery request);

    #endregion Public Methods
}
namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for managing applicants.
/// </summary>
public interface IApplicantsClient
{
    /// <summary>
    /// Get an applicant by their ID for editing.
    /// </summary>
    /// <param name="request">The query parameters for getting the applicant.</param>
    /// <returns>A <see cref="ApplicantForEdit"/>.</returns>

    #region Public Methods

    Task<ApiResponseWrapper<ApplicantForEdit>> GetApplicant(GetApplicantForEditQuery request);

    /// <summary>
    /// Get the references of an applicant.
    /// </summary>
    /// <param name="getApplicantReferencesQuery">
    /// The query parameters for getting the applicant references.
    /// </param>
    /// <returns>A <see cref="ApplicantReferencesResponse"/>.</returns>

    Task<ApiResponseWrapper<ApplicantReferencesResponse>> GetApplicantReferences(GetApplicantReferencesQuery getApplicantReferencesQuery);

    /// <summary>
    /// Get a list of applicants.
    /// </summary>
    /// <param name="request">The query parameters for getting the applicants.</param>
    /// <returns>A <see cref="ApplicantsResponse"/>.</returns>

    Task<ApiResponseWrapper<ApplicantsResponse>> GetApplicants(GetApplicantsQuery request);

    /// <summary>
    /// Create a new applicant.
    /// </summary>
    /// <param name="request">The command parameters for creating the applicant.</param>
    /// <returns>A <see cref="CreateApplicantResponse"/>.</returns>

    Task<ApiResponseWrapper<CreateApplicantResponse>> CreateApplicant(CreateApplicantCommand request);

    /// <summary>
    /// Update an existing applicant.
    /// </summary>
    /// <param name="request">The command parameters for updating the applicant.</param>
    /// <returns>A success message.</returns>

    Task<ApiResponseWrapper<string>> UpdateApplicant(UpdateApplicantCommand request);

    /// <summary>
    /// Delete an applicant.
    /// </summary>
    /// <param name="id">The ID of the applicant to delete.</param>
    /// <returns>A success message.</returns>

    Task<ApiResponseWrapper<string>> DeleteApplicant(string id);

    /// <summary>
    /// Export a list of applicants as a PDF file.
    /// </summary>
    /// <param name="request">The query parameters for exporting the applicants.</param>
    /// <returns>A <see cref="ExportApplicantsResponse"/>.</returns>

    Task<ApiResponseWrapper<ExportApplicantsResponse>> ExportAsPdf(ExportApplicantsQuery request);

    #endregion Public Methods
}
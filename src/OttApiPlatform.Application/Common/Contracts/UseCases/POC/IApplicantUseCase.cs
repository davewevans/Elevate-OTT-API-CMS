using OttApiPlatform.Application.Features.POC.Applicants.Commands.DeleteApplicant;
using OttApiPlatform.Application.Features.POC.Applicants.Commands.UpdateApplicant;
using OttApiPlatform.Application.Features.POC.Applicants.Queries.GetApplicantForEdit;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.POC;

public interface IApplicantUseCase
{
    #region Public Methods

    Task<Envelope<CreateApplicantResponse>> AddApplicant(CreateApplicantCommand request);

    Task<Envelope<string>> EditApplicant(UpdateApplicantCommand request);

    Task<Envelope<ApplicantsResponse>> GetApplicants(GetApplicantsQuery request);

    Task<Envelope<GetApplicantForEditQuery>> GetApplicant(GetApplicantForEditQuery request);

    Task<Envelope<string>> DeleteApplicant(DeleteApplicantCommand request);

    Task<Envelope<ApplicantReferencesResponse>> GetApplicantReferences(GetApplicantReferencesQuery request);

    Task<Envelope<ExportApplicantsResponse>> ExportAsPdf(ExportApplicantsQuery request);

    #endregion Public Methods
}
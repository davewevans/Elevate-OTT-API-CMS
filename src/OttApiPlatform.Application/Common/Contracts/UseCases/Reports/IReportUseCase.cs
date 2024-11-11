using OttApiPlatform.Application.Features.Reports.GetReportForEdit;
using OttApiPlatform.Application.Features.Reports.GetReports;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Reports;

public interface IReportUseCase
{
    #region Public Methods

    Task<Envelope<GetReportForEditQuery>> GetReport(GetReportForEditQuery request);

    Task<Envelope<ReportsResponse>> GetReports(GetReportsQuery request);

    #endregion Public Methods
}

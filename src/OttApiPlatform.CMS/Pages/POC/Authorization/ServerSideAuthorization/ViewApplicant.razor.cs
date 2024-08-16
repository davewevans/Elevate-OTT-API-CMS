namespace OttApiPlatform.CMS.Pages.POC.Authorization.ServerSideAuthorization;

public partial class ViewApplicant
{
    #region Public Properties

    [Parameter] public string ApplicantId { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }

    private ApplicantForEdit ApplicantForEditVm { get; set; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Authorization, "#", true),
            new(Resource.Server_Side_Authorization, "#", true),
            new(Resource.Applicants, "/poc/authorization/serverSideAuthorization/applicants"),
            new(Resource.View_Applicant, "#", true)
        });

        var responseWrapper = await ApplicantsClient.GetApplicant(new GetApplicantForEditQuery
        {
            Id = ApplicantId,
        });

        if (responseWrapper.IsSuccessStatusCode)
            ApplicantForEditVm = responseWrapper.Payload;
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    #endregion Protected Methods
}
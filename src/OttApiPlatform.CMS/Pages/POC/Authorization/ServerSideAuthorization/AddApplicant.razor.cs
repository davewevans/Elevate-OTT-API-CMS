namespace OttApiPlatform.CMS.Pages.POC.Authorization.ServerSideAuthorization;

public partial class AddApplicant
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private CreateApplicantCommand CreateApplicantCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Authorization, "#", true),
            new(Resource.Server_Side_Authorization, "#", true),
            new(Resource.Applicants, "/poc/authorization/serverSideAuthorization/applicants"),
            new(Resource.Add_Applicant, "#", true)
        });

        CreateApplicantCommand.DateOfBirth = DateTime.Now.AddYears(-18);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task SubmitForm()
    {
        var responseWrapper = await ApplicantsClient.CreateApplicant(CreateApplicantCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("poc/authorization/serverSideAuthorization/applicants");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}
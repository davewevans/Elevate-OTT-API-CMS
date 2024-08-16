namespace OttApiPlatform.CMS.Pages.POC.FluentValidation;

public partial class ClientSideValidation
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private CreateApplicantCommand CreateApplicantCommand { get; set; } = new();

    private bool IsTipsOpen { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Fluent_Validation, "#", true),
            new(Resource.Client_Side_Validation, "#", true),
            new(Resource.Add_Applicant, "#", true)
        });

        CreateApplicantCommand.DateOfBirth = DateTime.Now.AddYears(-18);
    }

    #endregion Protected Methods

    #region Private Methods

    private void TipsToggle()
    {
        IsTipsOpen = !IsTipsOpen;
    }

    private async Task SubmitForm()
    {
        var responseWrapper = await ApplicantsClient.CreateApplicant(CreateApplicantCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            CreateApplicantCommand = new CreateApplicantCommand();
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}
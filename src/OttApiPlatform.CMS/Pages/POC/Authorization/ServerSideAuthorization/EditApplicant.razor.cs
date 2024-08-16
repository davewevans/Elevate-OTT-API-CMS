namespace OttApiPlatform.CMS.Pages.POC.Authorization.ServerSideAuthorization;

public partial class EditApplicant
{
    #region Public Properties

    [Parameter] public string ApplicantId { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private ApplicantForEdit ApplicantForEditVm { get; set; } = new();
    private UpdateApplicantCommand UpdateApplicantCommand { get; set; }

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
            new(Resource.Edit_Applicant, "#", true)
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

    #region Private Methods

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateApplicantCommand = new UpdateApplicantCommand
            {
                Id = ApplicantForEditVm.Id,
                Ssn = ApplicantForEditVm.Ssn,
                FirstName = ApplicantForEditVm.FirstName,
                LastName = ApplicantForEditVm.LastName,
                DateOfBirth = ApplicantForEditVm.DateOfBirth,
                Height = ApplicantForEditVm.Height,
                Weight = ApplicantForEditVm.Weight,
            };
            var responseWrapper = await ApplicantsClient.UpdateApplicant(UpdateApplicantCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("poc/authorization/serverSideAuthorization/applicants");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    #endregion Private Methods
}
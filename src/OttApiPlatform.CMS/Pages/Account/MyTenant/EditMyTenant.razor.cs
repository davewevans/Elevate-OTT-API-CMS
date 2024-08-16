namespace OttApiPlatform.CMS.Pages.Account.MyTenant;

public partial class EditMyTenant
{
    #region Public Properties

    [Parameter] public string TenantName { get; set; }
    [CascadingParameter] public Task<AuthenticationState> AuthenticationState { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IMyTenantClient MyTenantClient { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private MyTenantForEdit MyTenantForEditVm { get; set; } = new();
    private UpdateMyTenantCommand UpdateMyTenantCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Edit_Tenant, "#", true)
        });

        await GetTenant();
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task GetTenant()
    {
        var responseWrapper = await MyTenantClient.GetTenant(new GetMyTenantForEditQuery
        {
            Name = TenantName
        });

        if (responseWrapper.IsSuccessStatusCode)
            MyTenantForEditVm = responseWrapper.Payload;
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateMyTenantCommand = new UpdateMyTenantCommand
            {
                Id = MyTenantForEditVm.Id,
                Name = MyTenantForEditVm.Name
            };
            var responseWrapper = await MyTenantClient.UpdateTenant(UpdateMyTenantCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateToNewSubDomain(UpdateMyTenantCommand.Name);
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
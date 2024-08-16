namespace OttApiPlatform.CMS.Pages.Tenants;

public partial class EditTenant
{
    #region Public Properties

    [Parameter] public string TenantId { get; set; }
    [CascadingParameter] public Task<AuthenticationState> AuthenticationState { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ITenantsClient TenantsClient { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private TenantForEdit TenantForEditVm { get; set; } = new();
    private UpdateTenantCommand UpdateTenantCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Tenants, "/tenants"),
            new(Resource.Edit_Tenant, "#", true)
        });

        await GetTenant();
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task GetTenant()
    {
        var responseWrapper = await TenantsClient.GetTenant(new GetTenantForEditQuery
        {
            Id = TenantId
        });

        if (responseWrapper.IsSuccessStatusCode)
            TenantForEditVm = responseWrapper.Payload;
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateTenantCommand = new UpdateTenantCommand
            {
                Id = TenantForEditVm.Id,
                Name = TenantForEditVm.Name
            };
            var responseWrapper = await TenantsClient.UpdateTenant(UpdateTenantCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("tenants");
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
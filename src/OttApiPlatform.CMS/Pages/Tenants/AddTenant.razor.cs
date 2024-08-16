namespace OttApiPlatform.CMS.Pages.Tenants;

public partial class AddTenant
{
    #region Public Properties

    [CascadingParameter] public Task<AuthenticationState> AuthenticationState { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private ITenantsClient TenantsClient { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private CreateTenantCommand CreateTenantCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Tenants, "/tenants"),
            new(Resource.Add_Tenant, "#", true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task SubmitForm()
    {
        var responseWrapper = await TenantsClient.CreateTenant(CreateTenantCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("/tenants");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}
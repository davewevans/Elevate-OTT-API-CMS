namespace OttApiPlatform.CMS.Pages.Identity.Roles;

public partial class AddRole
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IRolesClient RolesClient { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IPermissionsClient PermissionsClient { get; set; }

    private bool LoadingOnDemand { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private HashSet<PermissionItem> PermissionItems { get; set; } = new();
    private HashSet<PermissionItem> SelectedPermissionItems { get; set; }
    private CreateRoleCommand CreateRoleCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Identity, "Identity/roles"),
            new(Resource.Roles, "Identity/roles"),
            new(Resource.Add_Role, "#", true)
        });

        await InitializeTree(LoadingOnDemand);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task InitializeTree(bool loadingOnDemand)
    {
        LoadingOnDemand = loadingOnDemand;

        var responseWrapper = await PermissionsClient.GetPermissions(new GetPermissionsQuery
        {
            Id = null,
            LoadingOnDemand = LoadingOnDemand
        });

        if (responseWrapper.IsSuccessStatusCode)
        {
            PermissionItems = responseWrapper.Payload?.Permissions.ToHashSet();
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private async Task<HashSet<PermissionItem>> LoadServerData(PermissionItem parentNode)
    {
        var responseWrapper = await PermissionsClient.GetPermissions(new GetPermissionsQuery { Id = parentNode.Id, LoadingOnDemand = true });

        if (responseWrapper.IsSuccessStatusCode)
        {
            parentNode.Permissions = responseWrapper.Payload?.Permissions;
            return (parentNode.Permissions ?? throw new InvalidOperationException()).ToHashSet();
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return new HashSet<PermissionItem>();
    }

    private async Task SubmitForm()
    {
        CreateRoleCommand.SelectedPermissionIds = SelectedPermissionItems.Select(p => p.Id).ToList();

        var responseWrapper = await RolesClient.CreateRole(CreateRoleCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("identity/roles");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}
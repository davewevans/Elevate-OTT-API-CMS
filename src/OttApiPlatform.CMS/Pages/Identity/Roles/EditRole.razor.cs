namespace OttApiPlatform.CMS.Pages.Identity.Roles;

public partial class EditRole
{
    #region Public Properties

    [Parameter] public string RoleId { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IRolesClient RolesClient { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    private bool LoadingOnDemand { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private HashSet<PermissionItem> PermissionItems { get; set; } = new();
    private RoleForEdit RoleForEditVm { get; set; } = new();
    private HashSet<PermissionItem> SelectedPermissionItemsForEdit { get; set; }
    private HashSet<PermissionItem> SelectedPermissionItemsForView { get; set; }
    private UpdateRoleCommand UpdateRoleCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Identity, "Identity/roles"),
            new(Resource.Roles, "Identity/roles"),
            new(Resource.Edit_Role, "#", true)
        });

        await GetRole();

        await InitializeTree(LoadingOnDemand);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task GetRole()
    {
        var responseWrapper = await RolesClient.GetRole(new GetRoleForEditQuery
        {
            Id = RoleId
        });

        if (responseWrapper.IsSuccessStatusCode)
            RoleForEditVm = responseWrapper.Payload;
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    private async Task InitializeTree(bool loadingOnDemand)
    {
        SelectedPermissionItemsForView = new HashSet<PermissionItem>();
        SelectedPermissionItemsForEdit = new HashSet<PermissionItem>();
        PermissionItems = new HashSet<PermissionItem>();

        LoadingOnDemand = loadingOnDemand;

        var responseWrapper = await RolesClient.GetRolePermissions(new GetRolePermissionsForEditQuery()
        {
            PermissionId = null,
            RoleId = RoleId,
            LoadingOnDemand = loadingOnDemand
        });

        if (responseWrapper.IsSuccessStatusCode)
        {
            SelectedPermissionItemsForView = responseWrapper.Payload.SelectedPermissions.ToHashSet();
            SelectedPermissionItemsForEdit = responseWrapper.Payload.SelectedPermissions.ToHashSet();
            PermissionItems = responseWrapper.Payload.RequestedPermissions.ToHashSet();
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private async Task<HashSet<PermissionItem>> LoadServerData(PermissionItem parentNode)
    {
        var responseWrapper = await RolesClient.GetRolePermissions(new GetRolePermissionsForEditQuery
        {
            RoleId = RoleId,
            LoadingOnDemand = true,
            PermissionId = parentNode.Id
        });

        if (responseWrapper.IsSuccessStatusCode)
        {
            SelectedPermissionItemsForView = responseWrapper.Payload.SelectedPermissions.ToHashSet();
            parentNode.Permissions = responseWrapper.Payload.RequestedPermissions.ToList();
            return (parentNode.Permissions ?? throw new InvalidOperationException()).ToHashSet();
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return new HashSet<PermissionItem>();
    }

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateRoleCommand = new UpdateRoleCommand
            {
                Id = RoleForEditVm.Id,
                Name = RoleForEditVm.Name,
                IsDefault = RoleForEditVm.IsDefault,
                SelectedPermissionIds = LoadingOnDemand
                    ? SelectedPermissionItemsForEdit.Select(p => p.Id).ToList()
                    : SelectedPermissionItemsForView.Select(p => p.Id).ToList()
            };

            var responseWrapper = await RolesClient.UpdateRole(UpdateRoleCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("identity/roles");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    private void UpdateSelectedPermissions(bool isChecked, PermissionItem permission)
    {
        var permissionExist = SelectedPermissionItemsForEdit.Any(sp => sp.Id == permission.Id);
        if (isChecked)
        {
            if (!permissionExist)
                SelectedPermissionItemsForEdit.Add(permission);
        }
        else
        {
            if (permissionExist)
                SelectedPermissionItemsForEdit.RemoveWhere(x => x.Id == permission.Id);
        }
    }

    #endregion Private Methods
}
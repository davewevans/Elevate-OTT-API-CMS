namespace OttApiPlatform.CMS.Pages.Identity.Users;

public partial class PermissionsLookupDialog
{
    #region Public Properties

    [Parameter] public string UserId { get; set; }
    [Parameter] public string UserName { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IUsersClient UsersClient { get; set; }
    [Inject] private IPermissionsClient PermissionsClient { get; set; }

    private bool LoadingOnDemand { get; set; }
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    private GrantOrRevokeUserPermissionsCommand GrantOrRevokePermissionsCommand { get; } = new();
    private HashSet<PermissionItem> PermissionItems { get; set; } = new();
    private HashSet<PermissionItem> SelectedPermissionItemsForView { get; set; }
    private HashSet<PermissionItem> SelectedPermissionItemsForEdit { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        await InitializeTree(LoadingOnDemand);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task InitializeTree(bool loadingOnDemand)
    {
        SelectedPermissionItemsForView = new HashSet<PermissionItem>();
        SelectedPermissionItemsForEdit = new HashSet<PermissionItem>();
        PermissionItems = new HashSet<PermissionItem>();

        LoadingOnDemand = loadingOnDemand;

        var responseWrapper = await UsersClient.GetUserPermissions(new GetUserPermissionsQuery
        {
            UserId = UserId,
            LoadingOnDemand = LoadingOnDemand
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
            if (responseWrapper.ApiErrorResponse is { Status: 401 })
                MudDialog.Cancel();
        }
    }

    private async Task<HashSet<PermissionItem>> LoadServerData(PermissionItem parentNode)
    {
        var responseWrapper = await PermissionsClient.GetPermissions(new GetPermissionsQuery
        {
            Id = parentNode.Id,
            LoadingOnDemand = LoadingOnDemand
        });

        if (responseWrapper.IsSuccessStatusCode)
        {
            parentNode.Permissions = responseWrapper.Payload.Permissions.ToList() ?? new List<PermissionItem>();
            parentNode.Permissions.ForEach(p => p.IsChecked = SelectedPermissionItemsForView.Any(sp => p.Id == sp.Id));
            return (parentNode.Permissions ?? throw new InvalidOperationException()).ToHashSet();
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            if (responseWrapper.ApiErrorResponse is { Status: 401 })
                MudDialog.Cancel();
        }
        return new HashSet<PermissionItem>();
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

    private async Task SubmitForm()
    {
        GrantOrRevokePermissionsCommand.UserId = UserId;

        GrantOrRevokePermissionsCommand.SelectedPermissionIds = LoadingOnDemand
            ? SelectedPermissionItemsForEdit.Select(p => p.Id).ToList()
            : SelectedPermissionItemsForView.Select(p => p.Id).ToList();

        var responseWrapper = await UsersClient.GrantOrRevokeUserPermissions(GrantOrRevokePermissionsCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            MudDialog.Close();
            Snackbar.Add(responseWrapper.Payload, Severity.Success);
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            if (responseWrapper.ApiErrorResponse is { Status: 401 })
                MudDialog.Cancel();
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    #endregion Private Methods
}
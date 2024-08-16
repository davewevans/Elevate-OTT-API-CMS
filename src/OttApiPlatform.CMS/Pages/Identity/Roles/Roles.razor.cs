namespace OttApiPlatform.CMS.Pages.Identity.Roles;

public partial class Roles
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IRolesClient RolesClient { get; set; }

    private string SearchString { get; set; }
    private MudTable<RoleItem> Table { get; set; }
    private GetRolesQuery GetRolesQuery { get; } = new();
    private RolesResponse RolesResponse { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Identity, "Identity/roles"),
            new(Resource.Roles, "#",true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task<TableData<RoleItem>> ServerReload(TableState state)
    {
        GetRolesQuery.SearchText = SearchString;

        GetRolesQuery.PageNumber = state.Page + 1;

        GetRolesQuery.RowsPerPage = state.PageSize;

        GetRolesQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await RolesClient.GetRoles(GetRolesQuery);

        var tableData = new TableData<RoleItem>();

        if (responseWrapper.IsSuccessStatusCode)
        {
            RolesResponse = responseWrapper.Payload;
            tableData = new TableData<RoleItem>() { TotalItems = RolesResponse.Roles.TotalRows, Items = RolesResponse.Roles.Items };
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        return tableData;
    }

    private void FilterRoles(string searchString)
    {
        if (RolesResponse is null)
            return;
        SearchString = searchString;
        Table.ReloadServerData();
    }

    private void AddRole()
    {
        NavigationManager.NavigateTo("identity/roles/addRole");
    }

    private void EditRole(string id)
    {
        NavigationManager.NavigateTo($"identity/roles/editRole/{id}");
    }

    private async Task DeleteRole(string id)
    {
        var dialog = await DialogService.ShowAsync<DeleteConfirmationDialog>(Resource.Delete);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var responseWrapper = await RolesClient.DeleteRole(id);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                await Table.ReloadServerData();
            }
            else
            {
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    #endregion Private Methods
}
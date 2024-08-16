namespace OttApiPlatform.CMS.Pages.Identity.Users;

public partial class Users
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IUsersClient UsersClient { get; set; }

    private string SearchString { get; set; }
    private MudTable<UserItem> Table { get; set; }
    private List<RoleItem> RoleItems { get; set; } = new();
    private GetUsersQuery GetUsersQuery { get; } = new();
    private UsersResponse UsersResponse { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Identity, "Identity/users"),
            new(Resource.Users, "Identity/users", true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task<TableData<UserItem>> ServerReload(TableState state)
    {
        GetUsersQuery.SearchText = SearchString;

        GetUsersQuery.SelectedRoleIds = RoleItems.Select(r => r.Id).ToList();

        GetUsersQuery.PageNumber = state.Page + 1;

        GetUsersQuery.RowsPerPage = state.PageSize;

        GetUsersQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await UsersClient.GetUsers(GetUsersQuery);
        var tableData = new TableData<UserItem>();
        if (responseWrapper.IsSuccessStatusCode)
        {
            UsersResponse = responseWrapper.Payload;
            tableData = new TableData<UserItem>() { TotalItems = UsersResponse.Users.TotalRows, Items = UsersResponse.Users.Items };
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        return tableData;
    }

    private void FilterUsers(string searchString)
    {
        if (UsersResponse is null)
            return;
        SearchString = searchString;
        Table.ReloadServerData();
    }

    private async Task ShowRolesLookup()
    {
        var dialogParameters = new DialogParameters { { nameof(RolesLookupDialog.SelectedUserRoles), RoleItems.ToList() } };

        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<RolesLookupDialog>(Resource.Assign_Roles, dialogParameters, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            RoleItems = (List<RoleItem>)dialogResult.Data;
            FilterUsers(SearchString);
        }
    }

    private void AddUser()
    {
        NavigationManager.NavigateTo("identity/users/addUser");
    }

    private void EditUser(string id)
    {
        NavigationManager.NavigateTo($"identity/users/editUser/{id}");
    }

    private void ShowPermissionsLookupDialog(string userId, string userName)
    {
        var dialogParameters = new DialogParameters
        {
            { nameof(PermissionsLookupDialog.UserId), userId },
            { nameof(PermissionsLookupDialog.UserName), userName }
        };

        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        DialogService.Show<PermissionsLookupDialog>(Resource.Assign_Permissions, dialogParameters, dialogOptions);
    }

    private async Task DeleteUser(string id)
    {
        var dialog = await DialogService.ShowAsync<DeleteConfirmationDialog>(Resource.Delete);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var responseWrapper = await UsersClient.DeleteUser(id);

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
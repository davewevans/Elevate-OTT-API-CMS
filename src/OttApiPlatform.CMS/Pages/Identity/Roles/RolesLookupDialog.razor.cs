namespace OttApiPlatform.CMS.Pages.Identity.Roles;

public partial class RolesLookupDialog
{
    #region Public Properties

    [Parameter] public List<RoleItem> SelectedUserRoles { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IRolesClient RolesClient { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    private string SearchString { get; set; }
    private MudTable<RoleItem> Table { get; set; }
    private GetRolesQuery GetRolesQuery { get; } = new();
    private RolesResponse RolesResponse { get; set; } = new();

    #endregion Private Properties

    #region Private Methods

    private void AddToSelectedUserRoles(RoleItem item)
    {
        item.Checked = !item.Checked;

        if (item.Checked)
        {
            var assignedUserRoleItem = new RoleItem
            {
                Id = item.Id,
                Name = item.Name,
                IsStatic = item.IsStatic,
                IsDefault = item.IsDefault,
                Checked = item.Checked,
            };
            SelectedUserRoles.Add(assignedUserRoleItem);
        }
        else
        {
            SelectedUserRoles.RemoveAll(x => x.Id == item.Id);
        }
    }

    private void SelectAllVisibleRows(bool selected)
    {
        if (selected)
        {
            RolesResponse?.Roles.Items.ToList().ForEach(s => s.Checked = true);

            if (RolesResponse?.Roles.Items != null)
                SelectedUserRoles.AddRange(RolesResponse?.Roles.Items.Select(ur => new RoleItem
                {
                    Id = ur.Id,
                    Name = ur.Name,
                    Checked = ur.Checked,
                    IsDefault = ur.IsDefault,
                    IsStatic = ur.IsStatic
                }) ?? Array.Empty<RoleItem>());
        }
        else
        {
            RolesResponse?.Roles.Items.Where(r => r.Checked).ToList().ForEach(s => s.Checked = false);
            RolesResponse?.Roles.Items.ToList().ForEach(r => SelectedUserRoles.RemoveAll(sr => sr.Id == r.Id));
        }
    }

    private async Task<TableData<RoleItem>> ServerReload(TableState state)
    {
        GetRolesQuery.SearchText = SearchString;
        GetRolesQuery.PageNumber = state.Page + 1;
        GetRolesQuery.RowsPerPage = state.PageSize;
        GetRolesQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await RolesClient.GetRoles(GetRolesQuery);

        if (responseWrapper.IsSuccessStatusCode)
        {
            RolesResponse = responseWrapper.Payload;

            if (SelectedUserRoles != null && SelectedUserRoles.Any())
                RolesResponse?.Roles.Items.Where(r => SelectedUserRoles.Select(sr => sr.Id).Contains(r.Id)).ToList().ForEach(s => s.Checked = true);
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);

            if (responseWrapper.ApiErrorResponse is { Status: 401 })
                MudDialog.Cancel();

            return new TableData<RoleItem>();
        }

        return new TableData<RoleItem>() { TotalItems = RolesResponse?.Roles.TotalRows ?? 0, Items = RolesResponse?.Roles.Items ?? new List<RoleItem>() };
    }

    private void FilterRoles(string searchString)
    {
        if (RolesResponse is null)
            return;

        SearchString = searchString;
        Table.ReloadServerData();
    }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(SelectedUserRoles));
    }

    private void Closed(MudChip chip)
    {
        SelectedUserRoles.RemoveAll(sr => sr.Name.Trim() == chip.Text.Trim());
        Table.ReloadServerData();
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    #endregion Private Methods
}
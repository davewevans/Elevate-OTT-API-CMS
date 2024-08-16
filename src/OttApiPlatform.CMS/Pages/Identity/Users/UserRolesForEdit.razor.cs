namespace OttApiPlatform.CMS.Pages.Identity.Users;

public partial class UserRolesForEdit
{
    #region Public Properties

    [Parameter] public List<RoleItem> RoleItems { get; set; } = new();
    [Parameter] public EventCallback<List<RoleItem>> OnAssignedRolesChanged { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private IDialogService DialogService { get; set; }

    private string SearchText { get; set; }

    #endregion Private Properties

    #region Private Methods

    private bool FilterRoles(RoleItem roleItem)
    {
        return string.IsNullOrWhiteSpace(SearchText) || roleItem.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
    }

    private async Task ShowRolesLookupDialog()
    {
        var dialogParameters = new DialogParameters
        {
            { nameof(RolesLookupDialog.SelectedUserRoles), RoleItems.ToList() }
        };

        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await DialogService.ShowAsync<RolesLookupDialog>(Resource.Assign_Roles, dialogParameters, dialogOptions);
        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            SearchText = null;
            RoleItems = (List<RoleItem>)dialogResult.Data;
            await OnAssignedRolesChanged.InvokeAsync(RoleItems);
        }
    }

    private async Task RemoveRole(RoleItem item)
    {
        var dialog = await DialogService.ShowAsync<RemoveConfirmationDialog>(Resource.Remove);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
            RoleItems.Remove(item);
    }

    #endregion Private Methods
}
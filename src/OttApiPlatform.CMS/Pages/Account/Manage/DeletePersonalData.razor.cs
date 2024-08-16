namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class DeletePersonalData
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    private bool PasswordVisibility { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private DeletePersonalDataCommand DeletePersonalDataCommand { get; } = new();

    #endregion Private Properties

    #region Private Methods

    private async Task DeleteUserPersonalData()
    {
        var responseWrapper = await ManageClient.DeletePersonalData(DeletePersonalDataCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            MudDialog.Close(DialogResult.Ok("success"));
            Snackbar.Add(responseWrapper.Payload, Severity.Success);
            await AuthenticationService.Logout();
            NavigationManager.NavigateTo("account/login");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            if (responseWrapper.ApiErrorResponse is { Status: 401 })
                MudDialog.Cancel();
        }
    }

    private void TogglePasswordVisibility()
    {
        if (PasswordVisibility)
        {
            PasswordVisibility = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            PasswordVisibility = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    #endregion Private Methods
}
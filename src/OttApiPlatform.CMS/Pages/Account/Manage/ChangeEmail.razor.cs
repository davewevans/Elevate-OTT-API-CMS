namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class ChangeEmail
{
    #region Public Properties

    [CascadingParameter] public Task<AuthenticationState> AuthenticationState { get; set; }

    #endregion Public Properties

    #region Private Properties

    private string Email { get; set; }

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private ChangeEmailCommand ChangeEmailCommand { get; set; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await ManageClient.GetUser();

        if (responseWrapper.IsSuccessStatusCode)
        {
            var authenticationState = await AuthenticationState;
            Email = authenticationState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task Submit()
    {
        var responseWrapper = await ManageClient.ChangeEmail(ChangeEmailCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (!responseWrapper.Payload.EmailUnchanged)
                await Redirect(responseWrapper.Payload);
            else
                Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private async Task Redirect(ChangeEmailResponse responseWrapper)
    {
        if (responseWrapper.RequireConfirmedAccount)
            NavigationManager.NavigateTo(responseWrapper.DisplayConfirmAccountLink
                                             ? $"account/manage/emailChangeConfirmation/{responseWrapper.DisplayConfirmAccountLink}/{responseWrapper.EmailConfirmationUrl}"
                                             : $"account/manage/emailChangeConfirmation/{responseWrapper.DisplayConfirmAccountLink}");
        else
            await AuthenticationService.Login(responseWrapper.AuthResponse);
    }

    #endregion Private Methods
}
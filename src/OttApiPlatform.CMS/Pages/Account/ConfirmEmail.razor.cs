namespace OttApiPlatform.CMS.Pages.Account;

public partial class ConfirmEmail
{
    #region Private Fields

    private string _userId;
    private string _code;

    #endregion Private Fields

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }
    [Inject] private IReturnUrlProvider ReturnUrlProvider { get; set; }

    private ConfirmEmailCommand ConfirmEmailCommand { get; set; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        NavigationManager.TryGetQueryString("userId", out _userId);

        NavigationManager.TryGetQueryString("code", out _code);

        ConfirmEmailCommand = new ConfirmEmailCommand
        {
            Code = _code,
            UserId = _userId
        };

        var responseWrapper = await AccountsClient.ConfirmEmail(ConfirmEmailCommand);

        if (!responseWrapper.IsSuccessStatusCode)
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            return;
        }

        if (responseWrapper.Payload.AuthResponse != null)
        {
            await AuthenticationService.Login(responseWrapper.Payload.AuthResponse);
            NavigationManager.NavigateTo("/");
            return;
        }

        Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
        NavigationManager.NavigateTo("account/emailConfirmed");
    }

    #endregion Protected Methods
}
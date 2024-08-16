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

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload, Severity.Success);
            NavigationManager.NavigateTo("account/emailConfirmed");
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods
}
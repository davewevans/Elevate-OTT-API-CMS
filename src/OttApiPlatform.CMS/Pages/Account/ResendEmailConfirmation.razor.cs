namespace OttApiPlatform.CMS.Pages.Account;

public partial class ResendEmailConfirmation
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private ResendEmailConfirmationCommand ResendEmailConfirmationCommand { get; } = new();

    #endregion Private Properties

    #region Private Methods

    private async Task ResendConfirmation()
    {
        var responseWrapper = await AccountsClient.ResendEmailConfirmation(ResendEmailConfirmationCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            if (responseWrapper.Payload.RequireConfirmedAccount)
                NavigationManager.NavigateTo(responseWrapper.Payload.DisplayConfirmAccountLink
                                                 ? $"account/registerConfirmation/{responseWrapper.Payload.DisplayConfirmAccountLink}/{responseWrapper.Payload.EmailConfirmationUrl}"
                                                 : $"account/registerConfirmation/{responseWrapper.Payload.DisplayConfirmAccountLink}");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}
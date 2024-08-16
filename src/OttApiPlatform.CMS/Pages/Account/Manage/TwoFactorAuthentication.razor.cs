namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class TwoFactorAuthentication
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }

    private TwoFactorAuthenticationStateResponse TwoFactorAuthenticationStateResponse { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await ManageClient.Get2FaState();

        if (responseWrapper.IsSuccessStatusCode)
            TwoFactorAuthenticationStateResponse = responseWrapper.Payload;
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task DisableAuthenticator()
    {
        var dialog = await DialogService.ShowAsync<DisableTwoFactorAuthenticationDialog>();

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled && dialogResult.Data.ToString() == "success")
        {
            var responseWrapper = await ManageClient.Get2FaState();

            if (responseWrapper.IsSuccessStatusCode)
                TwoFactorAuthenticationStateResponse = responseWrapper.Payload;
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private async Task ResetAuthenticator()
    {
        var dialog = await DialogService.ShowAsync<ResetAuthenticatorDialog>();

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled && dialogResult.Data.ToString() == "success")
        {
            var responseWrapper = await ManageClient.Get2FaState();

            if (responseWrapper.IsSuccessStatusCode)
                TwoFactorAuthenticationStateResponse = responseWrapper.Payload;
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private async Task EnableAuthenticator()
    {
        var dialog = await DialogService.ShowAsync<EnableAuthenticatorDialog>();

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled && dialogResult.Data.ToString() == "success")
        {
            var responseWrapper = await ManageClient.Get2FaState();

            if (responseWrapper.IsSuccessStatusCode)
                TwoFactorAuthenticationStateResponse = responseWrapper.Payload;
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private async Task GenerateRecoveryCodes()
    {
        var dialog = await DialogService.ShowAsync<GenerateRecoveryCodesDialog>();

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled && dialogResult.Data.ToString() == "success")
        {
            var responseWrapper = await ManageClient.Get2FaState();

            if (responseWrapper.IsSuccessStatusCode)
                TwoFactorAuthenticationStateResponse = responseWrapper.Payload;
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}
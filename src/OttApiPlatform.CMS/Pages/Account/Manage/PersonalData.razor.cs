namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class PersonalData
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }

    #endregion Private Properties

    #region Private Methods

    private async Task DownloadPersonalData()
    {
        var responseWrapper = await ManageClient.DownloadPersonalData();

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (JsRuntime is Microsoft.JSInterop.WebAssembly.WebAssemblyJSRuntime webAssemblyJsRuntime)
                webAssemblyJsRuntime.InvokeUnmarshalled<string, string, byte[], bool>("BlazorDownloadFileFast", responseWrapper.Payload.FileName, responseWrapper.Payload.ContentType, responseWrapper.Payload.PersonalData);
            else
                // Fall back to the slow method if not in WebAssembly
                await JsRuntime.InvokeVoidAsync("BlazorDownloadFile", responseWrapper.Payload.FileName, responseWrapper.Payload.ContentType, responseWrapper.Payload.PersonalData);
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private void DeletePersonalData()
    {
        DialogService.Show<DeletePersonalData>();
    }

    #endregion Private Methods
}
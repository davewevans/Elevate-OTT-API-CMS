namespace OttApiPlatform.CMS.Components.ContentManagement;

public partial class Content
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }

    private bool IsTipsOpen { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
      
    }

    #endregion Protected Methods

    #region Private Methods
 
    private async void AppStateChanged(object sender, EventArgs args)
    {
        await InvokeAsync(StateHasChanged);
    }

    #endregion Private Methods
}

namespace OttApiPlatform.Gateway.Shared;

public partial class MainLayout
{
    #region Public Fields

    public bool IsDarkMode = true;

    #endregion Public Fields

    #region Private Fields

    public MudTheme DefaultTheme { get; set; } = new BpAdminDashboardTheme();

    #endregion Private Fields

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        await InvokeAsync(StateHasChanged);
    }

    #endregion Protected Methods

    #region Private Methods

    protected void DarkModeToggle()
    {
        IsDarkMode = !IsDarkMode;
    }

    #endregion Private Methods
}
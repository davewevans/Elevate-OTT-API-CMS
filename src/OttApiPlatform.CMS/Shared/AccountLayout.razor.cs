namespace OttApiPlatform.CMS.Shared;

public partial class AccountLayout
{
    #region Public Properties

    public bool IsDarkMode { get; set; } = true;
    public bool IsRightToLeft { get; set; }

    public MudTheme DefaultTheme { get; set; } = new CmsDashboardTheme();

    #endregion Public Properties

    #region Private Properties

    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private IAppStateManager AppStateManager { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        var culture = CultureInfo.CurrentCulture;

        IsRightToLeft = culture.TextInfo.IsRightToLeft;

        NavigationManager.LocationChanged += (obj, nav) => { StateHasChanged(); };

        AppStateManager.LoaderOverlayChanged += (obj, nav) => { StateHasChanged(); };
    }

    protected void DarkModeToggle()
    {
        IsDarkMode = !IsDarkMode;
    }

    #endregion Protected Methods
}
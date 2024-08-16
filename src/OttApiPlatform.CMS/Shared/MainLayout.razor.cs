namespace OttApiPlatform.CMS.Shared;

public partial class MainLayout
{
    #region Public Properties

    public MudTheme DefaultTheme { get; set; } = new CmsDashboardTheme();

    #endregion Public Properties

    #region Protected Properties

    protected bool DrawerOpen { get; set; } = true;
    protected bool IsRightToLeft { get; set; }

    [Inject] protected IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] protected IAppStateManager AppStateManager { get; set; }
    [Inject] protected ISnackbar Snackbar { get; set; }
    [Inject] protected NavigationManager NavigationManager { get; set; }
    protected bool IsDarkMode { get; set; } = true;

    #endregion Protected Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        var culture = CultureInfo.CurrentCulture;

        IsRightToLeft = culture.TextInfo.IsRightToLeft;

        BreadcrumbService.BreadcrumbChanged += (obj, nav) => { StateHasChanged(); };

        AppStateManager.LoaderOverlayChanged += (obj, nav) => { StateHasChanged(); };

        Snackbar.Configuration.PositionClass = !IsRightToLeft ? Defaults.Classes.Position.BottomRight : Defaults.Classes.Position.BottomLeft;
    }

    protected void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }

    protected void DarkModeToggle()
    {
        IsDarkMode = !IsDarkMode;
    }

    #endregion Protected Methods
}
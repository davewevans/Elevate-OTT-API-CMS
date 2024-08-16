namespace OttApiPlatform.CMS.Shared
{
    public partial class AppOverlay
    {
        #region Public Properties

        [Inject] public IAppStateManager AppStateManager { get; set; }
        [Parameter] public bool OverlayVisible { get; set; }

        #endregion Public Properties
    }
}
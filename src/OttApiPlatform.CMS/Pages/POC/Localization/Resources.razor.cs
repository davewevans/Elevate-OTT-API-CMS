namespace OttApiPlatform.CMS.Pages.POC.Localization;

public partial class Resources
{
    #region Private Properties

    private bool IsTipsOpen { get; set; }

    [Inject] private ILocalizationService LocalizationService { get; set; }

    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Localization, "#", true),
            new(Resource.Resources, "#", true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private void TipsToggle()
    {
        IsTipsOpen = !IsTipsOpen;
    }

    #endregion Private Methods
}
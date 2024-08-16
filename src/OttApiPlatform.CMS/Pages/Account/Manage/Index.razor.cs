namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class Index
{
    #region Private Properties

    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.My_Account, "#", true)
        });
    }

    #endregion Protected Methods
}
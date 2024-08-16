namespace OttApiPlatform.CMS.Pages.POC.Authorization;

public partial class TokenInfo
{
    #region Private Properties

    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

    private string SearchClaim { get; set; }
    private string SearchPermissions { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Authorization, "#", true),
            new(Resource.Token_Info, "#", true)
        });
    }

    #endregion Protected Methods
}
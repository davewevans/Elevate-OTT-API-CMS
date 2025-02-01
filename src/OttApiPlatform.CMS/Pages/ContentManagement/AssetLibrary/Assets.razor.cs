
using OttApiPlatform.CMS.Components.ContentManagement.Videos;

namespace OttApiPlatform.CMS.Pages.ContentManagement.AssetLibrary;

public partial class Assets
{
    #region Private Properties
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }

    private VideoList videoListChild;

    //[Inject] private IAssetsClient AssetsClient { get; set; }
    //private List<AssetForList> AssetsList { get; set; } = new();
    #endregion Private Properties
    #region Protected Methods
    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Content_Management, "#", true),
            new(Resource.Asset_Library, "#", true),
            new(Resource.Assets, "#", true)
        });
        //var responseWrapper = await AssetsClient.GetAssets(new GetAssetsQuery());
        //if (responseWrapper.IsSuccessStatusCode)
        //    AssetsList = responseWrapper.Payload;
        //else
        //    SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    #endregion Protected Methods
    public void VideoUploadCompleteHandler()
    {
        Console.WriteLine("VideoUploadCompleteHandler");
        // videoListChild.CallServerReload();
    }
}

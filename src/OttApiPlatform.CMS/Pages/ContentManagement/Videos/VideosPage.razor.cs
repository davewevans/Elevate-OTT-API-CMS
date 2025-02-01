using OttApiPlatform.CMS.Components.ContentManagement.Videos;

namespace OttApiPlatform.CMS.Pages.ContentManagement.Videos
{
    public partial class VideosPage
    {
        [Inject] private IBreadcrumbService? BreadcrumbService { get; set; }

        private VideoList videoListChild;

        protected override void OnInitialized()
        {
            BreadcrumbService?.SetBreadcrumbItems(new List<BreadcrumbItem>
            {
                new(Resource.Home, "/"),
                new(Resource.Videos, "#", true)
            });
        }

        public void VideoUploadCompleteHandler()
        {
            Console.WriteLine("VideoUploadCompleteHandler");
            videoListChild.CallServerReload();
        }
    }
}

using System.ComponentModel.DataAnnotations;
using OttApiPlatform.Domain.Enums.Content;

namespace OttApiPlatform.Domain.Entities.Content;

[Table("AssetImages")]
public class AssetImageModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public string? Name { get; set; }

    public AssetImageType AssetImageType { get; set; }

    [Url(ErrorMessage = "Invalid url.")]
    public string? Url { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    #region foreign keys
    [ForeignKey(nameof(VideoModel))]
    public Guid? VideoId { get; set; }

    [ForeignKey(nameof(LiveStreamModel))]
    public Guid? LiveStreamId { get; set; }

    [ForeignKey(nameof(PodcastModel))]
    public Guid? PodcastId { get; set; }
    #endregion

    #region Navigational Properties
    public VideoModel? Video { get; set; }
    public LiveStreamModel? LiveStream { get; set; }
    public PodcastModel? Podcast { get; set; }
    #endregion
}

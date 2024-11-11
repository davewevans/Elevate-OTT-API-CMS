using System.ComponentModel.DataAnnotations;
using OttApiPlatform.Domain.Enums.Content;

namespace OttApiPlatform.Domain.Entities.Content;

[Table("LiveStreams")]
public class LiveStreamModel : BaseAsset, IMustHaveTenant
{
    public Guid TenantId { get; set; }
    public string? Name { get; set; }
    public string? Sku { get; set; }
    public string? StreamUrl { get; set; }
    public string? StreamKey { get; set; }
    public StreamType StreamType { get; set; } = StreamType.Hls;
    public string? RtmpUrl { get; set; }
    public string? RtmpsUrl { get; set; }
    public LiveStreamStatus Status { get; set; }
    public VideoResolutionType VideoResolutionType { get; set; }
    public LatencyMode LatencyMode { get; set; }
    public float ReconnectWindow { get; set; }
    public string? MuxLiveStreamId { get; set; }
    public bool IsMuxLiveStream { get; set; }
    public DateTime LiveStreamCreatedAt { get; set; }
    public int MaxContinuousDuration { get; set; }
    public DateTime StartDateTime { get; set; }

    [StringLength(300)]
    public string? PreRegistrationText { get; set; }
    public string? Rating { get; set; }

    [StringLength(30)]
    public string? ButtonPurchaseText { get; set; }

    [StringLength(6, MinimumLength = 2, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string? TestLiveStreamPasscode { get; set; }

    #region foreign keys
    [ForeignKey(nameof(AuthorModel))]
    public Guid? AuthorId { get; set; }
    #endregion

    #region Navigational Properties
    public AuthorModel? Author { get; set; }
    //public ICollection<LiveStreamCategoryModel>? LiveStreamsCategories { get; set; }

    // public List<CommentModel>? Comments { get; set; }
    //public List<SubtitleModel>? Subtitles { get; set; }
    #endregion
}

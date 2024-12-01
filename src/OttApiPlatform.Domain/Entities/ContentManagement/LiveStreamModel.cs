using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("LiveStreams")]
public class LiveStreamModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }
    public string Name { get; set; }
    public string Sku { get; set; }
    public string StreamUrl { get; set; }
    public string StreamKey { get; set; }
    public StreamType StreamType { get; set; } = StreamType.Hls;
    public string RtmpUrl { get; set; }
    public string RtmpsUrl { get; set; }
    public LiveStreamStatus Status { get; set; }
    public VideoResolutionType VideoResolutionType { get; set; }
    public LatencyMode LatencyMode { get; set; }
    public float ReconnectWindow { get; set; }
    public string MuxLiveStreamId { get; set; }
    public bool IsMuxLiveStream { get; set; }
    public int MaxContinuousDuration { get; set; }
    public DateTime LiveStreamCreatedAt { get; set; }
    public DateTime LiveStreamEndedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [StringLength(300)]
    public string PreRegistrationText { get; set; }

    public string Rating { get; set; }

    [StringLength(30)]
    public string ButtonPurchaseText { get; set; }

    [StringLength(6, MinimumLength = 2, ErrorMessage = "The {0} value cannot exceed {1} characters.")]
    public string TestLiveStreamPasscode { get; set; }


    #region Navigational Properties

   
    #endregion
}

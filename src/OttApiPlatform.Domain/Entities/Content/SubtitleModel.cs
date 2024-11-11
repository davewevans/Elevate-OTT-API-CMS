namespace OttApiPlatform.Domain.Entities.Content;

public class SubtitleModel : BaseEntity
{
    public bool ClosedCaption { get; set; }

    public string? TrackId { get; set; } 

    public string? Passthrough { get; set; } 

    public Guid? LanguageCodeId { get; set; }

    #region foreign keys
    [ForeignKey(nameof(VideoModel))] 
    public Guid? VideoId { get; set; }

    [ForeignKey(nameof(LiveStreamModel))]
    public Guid? LiveStreamId { get; set; }
    #endregion

    #region Navigational Properties
    public VideoModel? Video { get; set; }
    public LiveStreamModel? LiveStream { get; set; }
    #endregion
}

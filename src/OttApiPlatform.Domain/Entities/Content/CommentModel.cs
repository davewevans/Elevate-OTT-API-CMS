namespace OttApiPlatform.Domain.Entities.Content;

[Table("Comments")]
public class CommentModel : BaseEntity
{
    // TODO link to OTT tenant
    // look at how Uscreen handles this

    public string? Comment { get; set; } 

    #region foreign keys
    [ForeignKey(nameof(VideoModel))]
    public Guid? VideoId { get; set; }
    #endregion

    #region Navigational Properties
    public VideoModel? Video { get; set; }
    #endregion
}

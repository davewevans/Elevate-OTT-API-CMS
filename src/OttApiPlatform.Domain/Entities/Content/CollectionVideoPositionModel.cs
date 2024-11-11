namespace OttApiPlatform.Domain.Entities.Content;

[Table("CollectionVideoPositions")]
public class CollectionVideoPositionModel : BaseEntity
{
    public Guid? CollectionId { get; set; }
    public Guid? VideoId { get; set; }
    public int Position { get; set; }

    #region Navigational Properties
    public CollectionModel? Collection { get; set; }
    public VideoModel? Video { get; set; }
    #endregion
}

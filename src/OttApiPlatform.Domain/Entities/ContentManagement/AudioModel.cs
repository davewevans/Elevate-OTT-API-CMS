namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Audios")]
public class AudioModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public string FileName { get; set; }

    public string Bitrate { get; set; }

    public int Duration { get; set; }
 
    public double FileSize { get; set; }

    public bool Encoded { get; set; }

    #endregion

    #region Foreign Key Properties

    public Guid AssetId { get; set; }

    #endregion

    #region Navigational Properties

    public AssetModel Asset { get; set; }


    #endregion
}

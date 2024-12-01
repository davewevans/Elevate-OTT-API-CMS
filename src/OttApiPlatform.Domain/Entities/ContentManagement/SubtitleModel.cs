namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Subtitles")]
public class SubtitleModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public bool ClosedCaption { get; set; }

    public string TrackId { get; set; }

    public string Passthrough { get; set; } 

    #endregion

    #region Foreign Key Properties

    public Guid? LanguageCodeId { get; set; }

    public Guid ContentId { get; set; } 

    #endregion

    #region Navigational Properties

    public ContentModel Content { get; set; }

    #endregion

}

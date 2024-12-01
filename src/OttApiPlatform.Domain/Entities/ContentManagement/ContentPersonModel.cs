namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("ContentPeople")]
public class ContentPersonModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public Guid ContentId { get; set; }

    public Guid PersonId { get; set; }

    public int Order { get; set; } 

    #endregion


    #region Navigational Properties 

    public ContentModel Content { get; set; }

    public PersonModel Person { get; set; } 

    #endregion

}

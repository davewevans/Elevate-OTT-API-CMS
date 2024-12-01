using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Series")]
public class SeriesModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid ContentId { get; set; }


    #region Navigation Properties

    public ContentModel Content { get; set; }

    public ICollection<SeasonModel> Seasons { get; set; }

    public ICollection<SeriesAssetModel> SeriesAssets { get; set; }

    #endregion
}

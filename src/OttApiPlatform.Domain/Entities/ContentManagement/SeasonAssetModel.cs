using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("SeasonAssets")]
public class SeasonAssetModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid AssetId { get; set; }
    public Guid SeasonId { get; set; }

    public int EpisodeNumber { get; set; }


    #region Navigational Properties 

    public AssetModel Asset { get; set; }

    public SeasonModel Season { get; set; } 

    #endregion
}

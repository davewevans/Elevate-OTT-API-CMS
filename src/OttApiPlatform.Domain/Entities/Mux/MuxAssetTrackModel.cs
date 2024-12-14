using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Entities.Mux;

[Table("MuxAssetTracks")]
public class MuxAssetTrackModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }
    public string TrackId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public int? MaxWidth { get; set; }
    public int? MaxHeight { get; set; }
    public double? MaxFrameRate { get; set; }
    public int? MaxChannels { get; set; }
    public double? Duration { get; set; }
    public bool Primary { get; set; }
    public string LanguageCode { get; set; }


    #region Foreign Key Properties

    public Guid MuxAssetId { get; set; }

    #endregion

    #region Navigational Properties 

    public MuxAssetModel MuxAsset { get; set; }

    #endregion
}

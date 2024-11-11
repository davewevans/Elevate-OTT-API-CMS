using OttApiPlatform.Domain.Entities.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Entities.Mux;

[Table("MuxPlaybackIds")]
public class MuxPlaybackIdModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }


    public string Policy { get; set; } = string.Empty;

    //
    // TODO map to id
    //
    public string PlaybackId { get; set; } = string.Empty;

    #region foreign keys
    [ForeignKey(nameof(VideoModel))]
    public Guid? VideoId { get; set; }

    [ForeignKey(nameof(LiveStreamModel))]
    public Guid? LiveStreamId { get; set; }
    #endregion

    #region navigational properties
    public VideoModel? Video { get; set; }
    public LiveStreamModel? LiveStream { get; set; }
    #endregion
}

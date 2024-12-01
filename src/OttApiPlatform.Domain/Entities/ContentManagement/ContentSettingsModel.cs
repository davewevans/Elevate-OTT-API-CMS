using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("ContentSettings")]
public class ContentSettingsModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    public bool AllowUsersToCreatePlaylist { get; set; }

    public bool RequireUsersSignInToAccessFreeContent { get; set; }

    public bool AllowMaturityRatings { get; set; }

    public bool AllowRatings { get; set; }

    public bool AllowComments { get; set; }

    public bool AllowReviews { get; set; }

    public bool AllowDownload { get; set; }

    public bool AllowSharing { get; set; }

    public bool AllowEmbedding { get; set; }

    public bool AllowLikes { get; set; } = true;

    public bool LikesCount { get; set; }

    public bool AllowDislikes { get; set; } = false;

    public bool DislikesCount { get; set; } = false;

    public bool AllowFavorites { get; set; } = true;

    public bool IsGeoRestricted { get; set; } 

    #endregion


    #region Navigational Properties

    public ICollection<ContentSettingsCountryModel> RestrictedCountries { get; set; }

    #endregion

}

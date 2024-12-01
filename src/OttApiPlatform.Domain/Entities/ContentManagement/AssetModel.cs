using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("Assets")]
public class AssetModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties

    public Guid TenantId { get; set; }

    [Required]
    public AssetType Type { get; set; }

    [Required]
    public string Url { get; set; }

    public string Passthrough { get; set; }

    public bool ClosedCaptions { get; set; }

    public string PublicPlaybackId { get; set; }

    public string SignedPlaybackId { get; set; }

    public bool IsTestAsset { get; set; }

    public AssetCreationStatus CreationStatus { get; set; }

    #endregion

    #region Foreign Key Properties

    public Guid? LanguageId { get; set; }  

    #endregion

    #region Navigational Properties

    public LanguageModel Language { get; set; }

    public VideoModel Video { get; set; }

    public AudioModel Audio { get; set; }

    public ImageModel Image { get; set; }

    public DocumentModel Document { get; set; }

    public ICollection<AssetStorageModel> AssetStorages { get; set; }

    public ICollection<PlaylistAssetModel> PlaylistAssets { get; set; }

    public ICollection<SeriesAssetModel> SeriesAssets { get; set; }

    public ICollection<SeasonAssetModel> SeasonAssets { get; set; }

    #endregion

}

using OttApiPlatform.Domain.Entities.Mux;
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

    public string DownloadUrl { get; set; }

    public string FileName { get; set; }

    public bool IsTemporary { get; set; } = true;

    public bool ClosedCaptions { get; set; }

    public bool IsTestAsset { get; set; }

    public AssetCreationStatus CreationStatus { get; set; }

    #endregion

    #region Foreign Key Properties

    public Guid? LanguageId { get; set; }

    public Guid? ImageId { get; set; }

    public Guid? DocumentId { get; set; }

    public Guid? MuxAssetId { get; set; }
    
    #endregion

    #region Navigational Properties

    public LanguageModel Language { get; set; }

    public ImageModel Image { get; set; }

    public DocumentModel Document { get; set; }

    public MuxAssetModel MuxAsset { get; set; }

    public ICollection<AssetStorageModel> AssetStorages { get; set; }

    public ICollection<CollectionsAssetModel> CollectionAssets { get; set; }

    public ICollection<SeriesAssetModel> SeriesAssets { get; set; }

    public ICollection<SeasonAssetModel> SeasonAssets { get; set; }


    #endregion

}

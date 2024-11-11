using OttApiPlatform.Domain.Common.DTOs;
using OttApiPlatform.Domain.Enums.Content;

namespace OttApiPlatform.Domain.DTOs;
public abstract class BaseAssetDto : AuditableDto
{
    public string StreamUrl { get; set; }

    public string BlobUrl { get; set; }

    public string SeoTitle { get; set; }

    public string SeoDescription { get; set; }

    public string Slug { get; set; }

    public string Passthrough { get; set; }
    public bool ClosedCaptions { get; set; }

    public string PublicPlaybackId { get; set; }
    public string SignedPlaybackId { get; set; }

    public string AssetId { get; set; }
    public AssetCreationStatus StreamCreationStatus { get; set; }

    public PublicationStatus PublicationStatus { get; set; }

    public ContentAccess ContentAccess { get; set; }

    public bool IsTestAsset { get; set; }

    public bool IsHostedOnMux { get; set; }

    public string FileName { get; set; }

    public string BlobName { get; set; }

    public virtual string Title { get; set; }

    public string ShortDescription { get; set; }

    public string FullDescription { get; set; }

    public string LanguageCode { get; set; }

    public TimeSpan? Duration { get; set; }

    public bool AllowDownload { get; set; }

    public string DownloadUrl { get; set; }

    public string TrailerVideoUrl { get; set; }

    public DateTime? UploadedOn { get; set; }

    public DateTime? ReleasedDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    // Images
    public string ThumbnailUrl { get; set; }

    public string PlayerImageUrl { get; set; }

    public string CatalogImageUrl { get; set; }

    public string FeaturedCatalogImageUrl { get; set; }

    public string AnimatedGifUrl { get; set; }
}

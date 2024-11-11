using System.ComponentModel.DataAnnotations;
using OttApiPlatform.Domain.Enums.Content;

namespace OttApiPlatform.Domain.Entities.Content;

public abstract class BaseAsset : BaseEntity
{
    [Url(ErrorMessage = "Invalid url.")]
    public string? StreamUrl { get; set; }

    [Url(ErrorMessage = "Invalid url.")]
    public string? BlobUrl { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? Slug { get; set; }

    public string? Passthrough { get; set; }
    public bool ClosedCaptions { get; set; }

    public string? PublicPlaybackId { get; set; }
    public string? SignedPlaybackId { get; set; }

    public string? AssetId { get; set; } 
    public AssetCreationStatus StreamCreationStatus { get; set; }

    public PublicationStatus PublicationStatus { get; set; }

    public ContentAccess ContentAccess { get; set; }

    public bool IsTestAsset { get; set; } 

    public bool IsHostedOnMux { get; set; } 

    public string? FileName { get; set; } 

    public string? BlobName { get; set; }

    [StringLength(300)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? ShortDescription { get; set; }

    [StringLength(2000)]
    public string? FullDescription { get; set; } 

    public string? LanguageCode { get; set; } 

    public TimeSpan? Duration { get; set; }

    public bool AllowDownload { get; set; }

    [Url(ErrorMessage = "Invalid url.")]
    public string? DownloadUrl { get; set; } 

    [Url(ErrorMessage = "Invalid url.")]
    public string? TrailerVideoUrl { get; set; }

    public DateTime? UploadedOn { get; set; }

    public DateTime? ReleasedDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    // Images
    [Url(ErrorMessage = "Invalid url.")]
    public string? ThumbnailUrl { get; set; }

    [Url(ErrorMessage = "Invalid url.")]
    public string? PlayerImageUrl { get; set; }

    [Url(ErrorMessage = "Invalid url.")]
    public string? CatalogImageUrl { get; set; }

    [Url(ErrorMessage = "Invalid url.")]
    public string? FeaturedCatalogImageUrl { get; set; }

    [Url(ErrorMessage = "Invalid url.")]
    public string? AnimatedGifUrl { get; set; }
}

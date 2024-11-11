using OttApiPlatform.Domain.DTOs;
using OttApiPlatform.Domain.Enums.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetVideos;

public class VideoItem : BaseAssetDto
{
    #region Public Properties

    public Guid Id { get; set; }
    public bool Mp4Support { get; set; }
    public Guid? TrailerVideoId { get; set; }
    public Guid? FeaturedCategoryVideoId { get; set; }  
    public Guid? AuthorId { get; set; }

    public bool HasOneTimePurchasePrice { get; set; }
    public double OneTimePurchasePrice { get; set; }
    public bool HasRentalPrice { get; set; }
    public RentalDuration RentalDuration { get; set; }
    public double RentalPrice { get; set; }

    public AuthorDto? Author { get; set; }
    public List<AssetImageDto>? VideoImages { get; set; }
    public List<TagDto>? Tags { get; set; }

    #endregion Public Properties


}

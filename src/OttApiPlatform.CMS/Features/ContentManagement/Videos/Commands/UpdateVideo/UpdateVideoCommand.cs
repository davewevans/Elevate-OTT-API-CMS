using OttApiPlatform.CMS.Models.DTOs;

namespace OttApiPlatform.CMS.Features.ContentManagement.Videos.Commands.UpdateVideo;

public class UpdateVideoCommand : BaseAssetDto
{
    #region Public Properties

    public Guid Id { get; set; }    
    public bool Mp4Support { get; set; } 

    public bool HasOneTimePurchasePrice { get; set; }
    public double OneTimePurchasePrice { get; set; }
    public bool HasRentalPrice { get; set; }
    public RentalDuration RentalDuration { get; set; }
    public double RentalPrice { get; set; }

    public Guid? TrailerVideoId { get; set; }
    public Guid? FeaturedCategoryVideoId { get; set; }

    //public IFormFile? PlayerImage { get; set; }
    //public IFormFile? CatalogImage { get; set; }
    //public IFormFile? FeaturedCatalogImage { get; set; }
    //public IFormFile? AnimatedGif { get; set; }

    public ImageState PlayerImageState { get; set; }
    public ImageState CatalogImageState { get; set; }
    public ImageState FeaturedCatalogImageState { get; set; }
    public ImageState AnimatedGifState { get; set; }

    public Guid? AuthorId { get; set; }

    public List<Guid>? CategoryIds { get; set; }

    #endregion Public Properties
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideosForAutoComplete;
using OttApiPlatform.CMS.Models.DTOs;

namespace OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideoForEdit;

public class VideoForEdit : BaseAssetDto, INotifyPropertyChanged
{
    #region Public Properties

    public event PropertyChangedEventHandler? PropertyChanged;

    private string? _title = string.Empty;

    public override string? Title
    {
        get
        {
            return _title;
        }

        set
        {
            if (value != _title)
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
    }

    public Guid Id { get; set; }    
    public bool Mp4Support { get; set; }
    public bool HasOneTimePurchasePrice { get; set; }
    public double OneTimePurchasePrice { get; set; }
    public bool HasRentalPrice { get; set; }
    public RentalDuration RentalDuration { get; set; }
    public double RentalPrice { get; set; }

    public Guid? TrailerVideoId { get; set; }
    public Guid? FeaturedCategoryVideoId { get; set; }

    public VideoItemForAutoComplete? TrailerVideo { get; set; }
    public VideoItemForAutoComplete? FeaturedCategoryVideo { get; set; }

    public AssetImageDto? PlayerImage { get; set; } = new();
    public AssetImageDto? CatalogImage { get; set; } = new();
    public AssetImageDto? FeaturedCatalogImage { get; set; } = new ();
    public AssetImageDto? AnimatedGif { get; set; } = new();

    public ImageState PlayerImageState { get; set; }
    public ImageState CatalogImageState { get; set; }
    public ImageState FeaturedCatalogImageState { get; set; }
    public ImageState AnimatedGifState { get; set; }

    public Guid? AuthorId { get; set; }
    public AuthorDto? Author { get; set; }
    public List<AssetImageDto>? VideoImages { get; set; }
    public List<Guid>? CategoryIds { get; set; }
    public List<TagDto>? Tags { get; set; }

    #endregion Public Properties


    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

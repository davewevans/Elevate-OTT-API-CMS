using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.ContentManagement;

[Table("People")]
public class PersonModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "Person name is a required field.")]
    [MaxLength(50, ErrorMessage = "Maximum length for the Name is 50 characters.")]
    public string Name { get; set; } 

    [MaxLength(2000, ErrorMessage = "Maximum length for the Bio is 2000 characters.")]
    public string Bio { get; set; } 

    [Url(ErrorMessage = "Image URL must be a valid URL.")]
    public string ImageUrl { get; set; }

    [MaxLength(300, ErrorMessage = "Maximum length for the SEO Title is {0} characters.")]
    public string SeoTitle { get; set; } 

    [MaxLength(500, ErrorMessage = "Maximum length for the SEO Description is 1000 characters.")]
    public string SeoDescription { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum length for the Slug is 50 characters.")]
    public string Slug { get; set; }


    #region Navigational Properties

    public ICollection<VideoModel> Videos { get; set; } = new List<VideoModel>();
    public ICollection<LiveStreamModel> LiveStreams { get; set; } = new List<LiveStreamModel>();
    public ICollection<AudioModel> Audios { get; set; } = new List<AudioModel>();

    public ICollection<ContentPersonModel> ContentPeople { get; set; }

    #endregion
}

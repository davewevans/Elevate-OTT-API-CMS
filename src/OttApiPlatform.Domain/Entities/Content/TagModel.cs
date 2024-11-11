using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.Content;

[Table("Tags")]
public class TagModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    [Required(ErrorMessage = "Tag name is a required field.")]
    [MaxLength(50, ErrorMessage = "Maximum length for the Name is 50 characters.")]
    public string? Name { get; set; }


    #region Navigational Properties

    public ICollection<VideoTagModel>? VideosTags { get; set; }

    #endregion
}

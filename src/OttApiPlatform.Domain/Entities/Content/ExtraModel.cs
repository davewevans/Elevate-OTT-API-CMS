using System.ComponentModel.DataAnnotations;

namespace OttApiPlatform.Domain.Entities.Content;

[Table("Extras")]
public class ExtraModel : BaseEntity 
{
    public string? Title { get; set; } 

    public string? Description { get; set; } 

    [Url]
    public string? Url { get; set; }  

    #region foreign keys
    [ForeignKey(nameof(VideoModel))]
    public Guid? VideoId { get; set; }
    #endregion

    #region Navigational Properties
    public VideoModel? Video { get; set; }
    #endregion
}

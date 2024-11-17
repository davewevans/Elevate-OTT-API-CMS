namespace OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideosForAutoComplete;

public class VideoItemForAutoComplete : IEquatable<VideoItemForAutoComplete>
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? ThumbnailUrl { get; set; }

    public bool Equals(VideoItemForAutoComplete? other)
    {
        return other != null && this.GetType() == other.GetType() && this.Id.Equals(other.Id);
    }

    public override bool Equals(object? obj) => Equals(obj as VideoItemForAutoComplete);

    public override int GetHashCode() => Id.GetHashCode();
}

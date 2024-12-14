using System.Reflection;

namespace OttApiPlatform.Domain.Enums;

public enum ContentAccess
{
    Free,
    Premium,
    Subscription,
    PayPerView,
    AdSupported,
    Exclusive,
    Trial
}

public enum ContentType
{
    Video,
    Audio,
}

public enum SwimLaneType
{
    Manual,
    Automatic
}

public enum SwimLaneCriteria
{
    RecentlyAdded,
    RecentlyPublished,
    RecentlyWatched,
    Recommended
}

public enum ContentStatus
{
    Draft,
    Published,
    Scheduled,
    Deleted
}

public enum ContentCategory
{
    Movie,
    Series,
    Episode,
    Clip,
    Trailer,
    LiveStream,
    Playlist,
    Channel,
    Collection,
    Bundle,
    Season,
    EpisodeGroup,
    ClipGroup,
    TrailerGroup,
    LiveStreamGroup,
    PlaylistGroup,
    ChannelGroup,
    CollectionGroup,
    BundleGroup,
    SeasonGroup
}

public enum PublicationStatus
{
    Unpublished,
    Published,
    Scheduled
}

public enum StreamType
{
    Hls,
    Dash,
    SmoothStreaming
}

public enum VideoResolutionType
{
    Sd,
    Hd,
    Fhd,
    Qhd,   // Quad High Definition (1440p, sometimes called 2K)
    Uhd,   // Ultra High Definition (2160p, 4K)
    EightK // 8K Ultra High Definition (4320p)
}

public enum AssetCreationStatus
{
    None,
    Preparing,
    Ready,
    Errored,
    Deleted
}

public enum DirectUploadStatus
{
    None,
    Waiting,
    UploadCreated,
    UploadAssetCreated,
    Cancelled,
    TimedOut,
    AssetReady,
    Errored
}

public enum AssetType
{
    Video,
    Audio,
    Image,
    Document,
    VideoTrailer,
    VideoEpisode,
    VideoClip,
    VideoBonusContent,
    AudioTrailer,
    AudioEpisode,
    AudioClip,
    AudioBonusContent,
}

public enum AssetStatus
{
    None,
    Processing,
    Ready,
    Errored,
    Deleted
}

public enum AssetImageType
{
    Thumbnail,
    PlayerImage,
    CatalogImage,
    FeaturedCatalogImage,
    AnimatedGif
}

public enum ImageState
{
    Unchanged,
    Added,
    Removed
}

public enum AssetImageSize
{
    Small,
    Medium,
    Large
}

public enum VodStreamingService
{
    Managed, // Managed by the platform
    UserProvided,
    Mux,
    DaCast,
    Wowza,
    Brightcove,
    Azure,
}


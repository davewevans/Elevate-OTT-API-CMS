namespace OttApiPlatform.Domain.Enums.Content;

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
    Audio
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
    Fhd
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
    Image,
    Audio,
    Document
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

public enum AssetImageStatus
{
    None,
    Processing,
    Ready,
    Errored,
    Deleted
}



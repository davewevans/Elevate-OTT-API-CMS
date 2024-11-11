namespace OttApiPlatform.Domain.Enums.Content;

public enum ContentAccess
{
    Free,
    Premium
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

public enum LiveStreamStatus
{
    None,
    Preparing,
    Ready,
    Errored,
    Deleted
}

public enum LatencyMode
{
    Low,
    Reduced,
    Standard
}
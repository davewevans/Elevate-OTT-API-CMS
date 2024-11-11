using System.Text.Json.Serialization;

namespace OttApiPlatform.Application.Common.Models.Mux;

public class MuxWebhookRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("passthrough")]
    public Guid? Passthrough { get; set; }

    [JsonPropertyName("object")]
    public Object? Object { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("environment")]
    public Environment? Environment { get; set; }

    [JsonPropertyName("data")]
    public Data? Data { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = string.Empty;

    [JsonPropertyName("accessor_source")]
    public string AccessorSource { get; set; } = string.Empty;

    [JsonPropertyName("accessor")]
    public string Accessor { get; set; } = string.Empty;

    [JsonPropertyName("request_id")]
    public string RequestId { get; set; } = string.Empty;
}

public class Object
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class Environment
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class Data
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("tracks")]
    public List<Track>? Tracks { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("max_stored_resolution")]
    public string MaxStoredResolution { get; set; } = string.Empty;

    [JsonPropertyName("max_stored_frame_rate")]
    public double MaxStoredFrameRate { get; set; }

    [JsonPropertyName("duration")]
    public double Duration { get; set; }

    [JsonPropertyName("aspect_ratio")]
    public string AspectRatio { get; set; } = string.Empty;

    [JsonPropertyName("new_asset_settings")]
    public NewAssetSettings? NewAssetSettings { get; set; }

    [JsonPropertyName("cors_origin")]
    public string CorsOrigin { get; set; } = string.Empty;

    [JsonPropertyName("timeout")]
    public int Timeout { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("test")]
    public bool Test { get; set; }

    [JsonPropertyName("errors")]
    public Error? Errors { get; set; }

    [JsonPropertyName("playback_ids")]
    public List<PlaybackId>? PlaybackIds { get; set; }

    [JsonPropertyName("static_renditions")]
    public StaticRendition? StaticRenditions { get; set; }

    [JsonPropertyName("passthrough")]
    public string Passthrough { get; set; } = string.Empty;

    [JsonPropertyName("mp4_support")]
    public string Mp4Support { get; set; } = string.Empty;

    [JsonPropertyName("stream_key")]
    public string StreamKey { get; set; } = string.Empty;

    [JsonPropertyName("reconnect_window")]
    public float ReconnectWindow { get; set; }

    [JsonPropertyName("max_continuous_duration")]
    public int MaxContinuousDuration { get; set; }

    [JsonPropertyName("latency_mode")]
    public string LatencyMode { get; set; } = string.Empty;
}

public class PlaybackId
{
    [JsonPropertyName("policy")]
    public string Policy { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}

public class Track
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("max_width")]
    public int MaxWidth { get; set; }

    [JsonPropertyName("max_height")]
    public int MaxHeight { get; set; }

    [JsonPropertyName("max_frame_rate")]
    public double MaxFrameRate { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("duration")]
    public double Duration { get; set; }
}

public class NewAssetSettings
{
    [JsonPropertyName("playback_policy")]
    public string PlaybackPolicy { get; set; } = string.Empty;

    [JsonPropertyName("mp4_support")]
    public string Mp4Support { get; set; } = string.Empty;

    [JsonPropertyName("passthrough")]
    public string Passthrough { get; set; } = string.Empty;

    [JsonPropertyName("input")]
    public Input[]? Input { get; set; }

    [JsonPropertyName("normalize_audio")]
    public bool NormalizeAudio { get; set; }

    [JsonPropertyName("master_access")]
    public string MasterAccess { get; set; } = string.Empty;

    [JsonPropertyName("test")]
    public bool Test { get; set; }
}

public class Input
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("overlay_settings")]
    public OverlaySettings? OverlaySettings { get; set; }

    [JsonPropertyName("start_time")]
    public int StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public int EndTime { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("text_type")]
    public string TextType { get; set; } = string.Empty;

    [JsonPropertyName("language_code")]
    public string LanguageCode { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("closed_captions")]
    public bool ClosedCaptions { get; set; }

    [JsonPropertyName("passthrough")]
    public string Passthrough { get; set; } = string.Empty;
}

public class OverlaySettings
{
    [JsonPropertyName("vertical_align")]
    public string VerticalAlign { get; set; } = string.Empty;

    [JsonPropertyName("vertical_margin")]
    public string VerticalMargin { get; set; } = string.Empty;

    [JsonPropertyName("horizontal_align")]
    public string HorizontalAlign { get; set; } = string.Empty;

    [JsonPropertyName("horizontal_margin")]
    public string HorizontalMargin { get; set; } = string.Empty;

    [JsonPropertyName("width")]
    public string Width { get; set; } = string.Empty;

    [JsonPropertyName("height")]
    public string Height { get; set; } = string.Empty;

    [JsonPropertyName("opacity")]
    public string Opacity { get; set; } = string.Empty;
}

public class StaticRendition
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("files")]
    public List<File>? Files { get; set; }
}

public class File
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("ext")]
    public string Ext { get; set; } = string.Empty;

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("bitrate")]
    public int Bitrate { get; set; }

    [JsonPropertyName("filesize")]
    public int Filesize { get; set; }
}

public class Error
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("messages")]
    public List<string>? Messages { get; set; }
}

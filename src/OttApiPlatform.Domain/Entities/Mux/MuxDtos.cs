namespace OttApiPlatform.Domain.Entities.Mux;

public class MuxAssetResponse
{
    public string Id { get; set; }
    public string Status { get; set; }
    public List<MuxPlaybackId> PlaybackIds { get; set; }
    public double Duration { get; set; }
}

public class MuxPlaybackId
{
    public string Id { get; set; }
    public string Policy { get; set; }
}

public class MuxPlaybackIdResponse
{
    public MuxPlaybackId Data { get; set; }
}

public class MuxAssetListResponse
{
    public List<MuxAssetResponse> Data { get; set; }
}
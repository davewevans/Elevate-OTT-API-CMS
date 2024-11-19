namespace OttApiPlatform.Domain.Enums.Content
{
    public enum LiveStreamStatus
    {
        None,
        Preparing,
        Ready,
        Errored,
        Deleted
    }

    public enum LiveStreamType
    {
        Hls,
        Dash,
        SmoothStreaming
    }

    public enum LatencyMode
    {
        Low,
        Reduced,
        Standard
    }

    public enum LiveStreamStatusType
    {
        None,
        Starting,
        Running,
        Stopping,
        Stopped,
        Errored
    }

    public enum LiveStreamStatusReason
    {
        None,
        StreamStarted,
        StreamStopped,
        StreamErrored
    }
}

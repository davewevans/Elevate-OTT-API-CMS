namespace OttApiPlatform.Infrastructure.Services.DateAndTime;

public class UtcTimeService : IUtcTimeService
{
    #region Public Methods

    public long GetUnixTimeSeconds() => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

    public long GetUnixTimeMilliseconds() => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

    public DateTime GetUtcNow() => DateTime.UtcNow;

    #endregion Public Methods
}
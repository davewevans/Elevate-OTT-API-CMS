namespace OttApiPlatform.Infrastructure.Services.DateAndTime;

/// <summary>
/// Provides a fixed and consistent value for the current UTC time that does not change throughout
/// the lifetime of the request which can improve consistency, determinism, and reliability of the
/// application, especially in situations where time-sensitive information is used or tested.
/// </summary>
public class FrozenUtcTimeService : IUtcTimeService
{
    #region Private Fields

    private readonly DateTime _utcTime;

    #endregion Private Fields

    #region Public Constructors

    public FrozenUtcTimeService(DateTime utcTime)
    {
        _utcTime = utcTime.Kind == DateTimeKind.Utc ? utcTime : throw new ArgumentException(Resource.The_time_provided_must_be_in_UTC_format, nameof(utcTime));
    }

    public FrozenUtcTimeService(IUtcTimeService utcTime)
    {
        _utcTime = utcTime.GetUtcNow();
    }

    #endregion Public Constructors

    #region Public Methods

    public DateTime GetUtcNow() => _utcTime;
    public long GetUnixTimeSeconds() => new DateTimeOffset(_utcTime).ToUnixTimeSeconds();
    public long GetUnixTimeMilliseconds() => new DateTimeOffset(_utcTime).ToUnixTimeMilliseconds();

    #endregion Public Methods
}
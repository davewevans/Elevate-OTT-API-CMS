namespace ElevateOTT.Infrastructure.Services;

public static class DateTimeProvider
{
    #region Public Properties

    public static DateTime Now => DateTime.Now;
    public static DateTime UtcNow => DateTime.UtcNow;

    public static long GetUnixTimeMilliseconds()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }

    #endregion Public Properties
}
namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.DateAndTime;

/// <summary>
/// Represents an interface for a service that provides the current UTC time.
/// </summary>
public interface IUtcTimeService
{
    #region Public Methods

    /// <summary>
    /// Gets the current UTC time.
    /// </summary>
    /// <returns>A DateTime object representing the current UTC time.</returns>
    DateTime GetUtcNow();

    /// <summary>
    /// Gets the current time in Unix format (milliseconds since the Unix epoch).
    /// </summary>
    /// <returns>
    /// A long integer representing the current time in Unix format (milliseconds since the Unix epoch).
    /// </returns>
    long GetUnixTimeMilliseconds();

    /// <summary>
    /// Gets the current time in Unix format (seconds since the Unix epoch).
    /// </summary>
    /// <returns>
    /// A long integer representing the current time in Unix format (seconds since the Unix epoch).
    /// </returns>
    long GetUnixTimeSeconds();

    #endregion Public Methods
}
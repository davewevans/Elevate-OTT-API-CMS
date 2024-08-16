namespace OttApiPlatform.Domain.Enums;

/// <summary>
/// Represents the unit of time used in a schedule.
/// </summary>
public enum UnitOfTime
{
    /// <summary>
    /// Represents minutes as the unit of time.
    /// </summary>
    Minutes = 0,

    /// <summary>
    /// Represents hours as the unit of time.
    /// </summary>
    Hours = 1,

    /// <summary>
    /// Represents days as the unit of time.
    /// </summary>
    Days = 2,

    /// <summary>
    /// Represents months as the unit of time.
    /// </summary>
    Month = 3
}
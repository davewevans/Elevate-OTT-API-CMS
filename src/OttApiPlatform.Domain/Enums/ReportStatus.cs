namespace OttApiPlatform.Domain.Enums;

/// <summary>
/// Enum representing the status of a report.
/// </summary>
public enum ReportStatus
{
    /// <summary>
    /// The report is pending.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// The report is in progress.
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// The report is completed.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// The report failed.
    /// </summary>
    Failed = 4,

    /// <summary>
    /// The report is not found.
    /// </summary>
    NotFound = 5
}
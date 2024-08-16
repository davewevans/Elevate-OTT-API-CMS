namespace OttApiPlatform.Domain.Enums;

/// <summary>
/// Represents the status of a file.
/// </summary>
public enum FileStatus
{
    /// <summary>
    /// The file has not been modified.
    /// </summary>
    Unchanged = 1,

    /// <summary>
    /// The file has been modified.
    /// </summary>
    Modified = 2,

    /// <summary>
    /// The file has been deleted.
    /// </summary>
    Deleted = 3
}
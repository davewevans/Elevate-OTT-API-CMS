namespace OttApiPlatform.Application.Common.Models;

/// <summary>
/// Abstract base class for filterable queries.
/// </summary>
public abstract class FilterableQuery
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the search text to filter the result set.
    /// </summary>
    public string SearchText { get; set; }

    /// <summary>
    /// Gets or sets the property to sort the result set by.
    /// </summary>
    public string SortBy { get; set; }

    /// <summary>
    /// Gets or sets the page number to return.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of records for each page.
    /// </summary>
    /// <remarks>This property is used to limit the number of records returned per page.</remarks>
    public int RowsPerPage { get; set; }

    #endregion Public Properties
}
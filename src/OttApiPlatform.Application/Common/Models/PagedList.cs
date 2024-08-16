namespace OttApiPlatform.Application.Common.Models;

/// <summary>
/// Represents a paged list of items.
/// </summary>
/// <typeparam name="T">The type of items in the paged list.</typeparam>
public class PagedList<T>
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
    /// </summary>
    public PagedList()
    {
        Items = new List<T>();
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages in the paged list.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the total number of rows per page.
    /// </summary>
    public int TotalRowsPerPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of rows in the paged list.
    /// </summary>
    public int TotalRows { get; set; }

    /// <summary>
    /// Gets or sets the items in the paged list.
    /// </summary>
    public IReadOnlyList<T> Items { get; set; }

    #endregion Public Properties
}
namespace OttApiPlatform.CMS.Models;

public abstract class FilterableQuery
{
    #region Public Properties

    public string SearchText { get; set; }

    public string SortBy { get; set; }

    public int PageNumber { get; set; }

    public int RowsPerPage { get; set; }

    #endregion Public Properties
}
namespace OttApiPlatform.CMS.Models;

public class PagedList<T>
{
    #region Public Constructors

    public PagedList()
    {
        Items = new List<T>();
    }

    #endregion Public Constructors

    #region Public Properties

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRowsPerPage { get; set; }
    public int TotalRows { get; set; }
    public IList<T> Items { get; set; }

    #endregion Public Properties
}
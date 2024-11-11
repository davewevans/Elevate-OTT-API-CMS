using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Domain.Common.Models;

public abstract class FilterableQuery
{
    #region Public Properties

    public string SearchText { get; set; } = string.Empty;

    public string SortBy { get; set; } = string.Empty;

    public int PageNumber { get; set; }

    /// <summary>
    /// Maximum total number of records for each page.
    /// </summary>
    public int RowsPerPage { get; set; }

    /// <summary>
    /// Actual total number of records of the selected page.
    /// </summary>
    public int SelectedRowsPerPage { get; set; }

    #endregion Public Properties
}

namespace OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategoriesForAutoComplete;

public class CategoriesForAutoCompleteResponse
{
    #region Public Properties

    public PagedList<CategoryItemForAutoComplete>? Categories { get; set; }

    #endregion Public Properties
}

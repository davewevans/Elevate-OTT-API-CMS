using System.Diagnostics.CodeAnalysis;
using OttApiPlatform.CMS.Features.ContentManagement.Categories.Queries.GetCategoriesForAutoComplete;

namespace OttApiPlatform.CMS.Features.ContentManagement.Videos.Queries.GetVideosForAutoComplete;

public class CategoryItemForAutoCompleteEqualityComparer : IEqualityComparer<CategoryItemForAutoComplete>
{
    public bool Equals(CategoryItemForAutoComplete? x, CategoryItemForAutoComplete? y)
    {
        return x != null && y != null && x.Id.Equals(y.Id);
    }

    public int GetHashCode([DisallowNull] CategoryItemForAutoComplete obj)
    {
        return obj.Id.GetHashCode();
    }
}

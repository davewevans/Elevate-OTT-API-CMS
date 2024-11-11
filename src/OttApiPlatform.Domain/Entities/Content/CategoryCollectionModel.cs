namespace OttApiPlatform.Domain.Entities.Content;


// TODO implement IAuditable


[Table("CategoriesCollections")]
public class CategoryCollectionModel : IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid CollectionId { get; set; }

    public CategoryModel? Category { get; set; }

    public CollectionModel? Collection { get; set; }
}

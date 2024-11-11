using OttApiPlatform.Domain.Enums.DirectSubscribers;

namespace OttApiPlatform.Domain.Entities.Products;

[Table("ProductFamilies")]
public class ProductFamilyModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public ProductFamilyModel()
    {
        ProductItems = new List<ProductItemModel>();
    }
    public string? ChargebeeItemFamilyId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public ProductFamilyStatus Status { get; set; }

    public long ResourceVersion { get; set; }

    public string? Object { get; set; }

    #region foreign keys
  
    #endregion

    #region navigational properties
   
    public List<ProductItemModel> ProductItems { get; set; }
    #endregion
}

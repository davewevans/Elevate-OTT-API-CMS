
namespace OttApiPlatform.Domain.Entities.Products;

[Table("ProductItems")]
public class ProductItemModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public ProductItemModel()
    {
        ItemPrices = new List<ItemPriceModel>();
    }

    #region Chargebee Properties
    public ProductItemType Type { get; set; }

    public string? ItemId { get; set; }

    public string? ItemFamilyId { get; set; }

    public string? Name { get; set; }

    public string? ExternalName { get; set; }

    public string? Description { get; set; }

    public ItemStatus Status { get; set; }

    public string? ChargeModel { get; set; }

    public int FreeQuantity { get; set; }

    public string? Object { get; set; }

    public string? CurrencyCode { get; set; }

    public bool EnabledInHostedPages { get; set; }

    public bool EnabledInPortal { get; set; }

    public bool EnableForCheckout { get; set; }

    public bool Metered { get; set; }

    public bool Archivable { get; set; }

    public bool Taxable { get; set; }

    public string? Channel { get; set; }

    public string? Unit { get; set; }

    public string? UsageCalculation { get; set; }

    public bool IsShippable { get; set; }

    public bool IsGiftable { get; set; }

    public long ResourceVersion { get; set; }

    public string? RedirectUrl { get; set; }

    public string? ItemApplicability { get; set; }
    #endregion

    public string? Title { get; set; }

    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    public string? Sku { get; set; }

    public string Image { get; set; }

    public SubscriptionAvailability SubscriptionAvailability { get; set; }

    #region foreign keys
    [ForeignKey(nameof(ProductFamilyModel))]
    public Guid? ProductFamilyId { get; set; }
    #endregion

    #region navigational properties
    public ProductFamilyModel? ProductFamily { get; set; }
    public List<ItemPriceModel> ItemPrices { get; set; }
    #endregion
}

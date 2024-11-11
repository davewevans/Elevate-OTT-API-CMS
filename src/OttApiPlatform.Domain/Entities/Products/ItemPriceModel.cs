using OttApiPlatform.Domain.Enums.DirectSubscribers;

namespace OttApiPlatform.Domain.Entities.Products;

[Table("ItemPrices")]
public class ItemPriceModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    #region Chargebee Properties
    public string? ChargebeeItemPriceId { get; set; }

    public string? ItemId { get; set; }

    public string? ItemFamilyId { get; set; }

    public string? Name { get; set; }

    public ItemStatus Status { get; set; }

    public string? ExternalName { get; set; }

    public string? PricingModel { get; set; }

    public long ResourceVersion { get; set; }

    public bool Archivable { get; set; }

    
    public double Price { get; set; }

    public string? PricingInDecimal { get; set; }

    public int Period { get; set; }

    public BillingPeriodUnit PeriodUnit { get; set; }

    public string? CurrencyCode { get; set; }

    public string? Object { get; set; }

    public int FreeQuantity { get; set; }

    public string? FreeQuantityInDecimal { get; set; }

    public string? Channel { get; set; }

    public bool IsTaxable { get; set; }

    public ProductItemType ProductItemType { get; set; }

    public bool ShowDescriptionInInvoices { get; set; }

    public bool ShowDescriptionInQuotes { get; set; }
    #endregion


    #region foreign keys
    [ForeignKey(nameof(ProductItemModel))]
    public Guid? ProductItemId { get; set; }
    #endregion

    #region navigational properties
    public ProductItemModel? ProductItem { get; set; }
    #endregion
}

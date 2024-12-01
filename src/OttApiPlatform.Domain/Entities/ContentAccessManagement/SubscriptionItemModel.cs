namespace OttApiPlatform.Domain.Entities.ContentAccessManagement;

[Table("SubscriptionItems")]
public class SubscriptionItemModel : BaseEntity
{
    #region Public Properties

    public string ItemPriceId { get; set; } = string.Empty;

    public ProductItemType ItemType { get; set; }

    public int Quantity { get; set; }


    public double UnitPrice { get; set; }


    public double Amount { get; set; }

    public int FreeQuantity { get; set; }

    public string Object { get; set; } = string.Empty;

    #endregion
    

    #region Foreign Key Properties

    public Guid? SubscriptionId { get; set; }

    #endregion


    #region Navigational Properties 

    public SubscriptionModel Subscription { get; set; }


    #endregion
}

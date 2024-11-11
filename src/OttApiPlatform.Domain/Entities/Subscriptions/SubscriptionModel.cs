using OttApiPlatform.Domain.Enums.DirectSubscribers;

namespace OttApiPlatform.Domain.Entities.Subscriptions;

[Table("Subscriptions")]
public class SubscriptionModel : BaseEntity
{
    public string Name { get; set; } = string.Empty;


    #region Chargebee Properties
    public string ChargebeeSubscriptionId { get; set; } = string.Empty;

    public int BillingPeriod { get; set; }

    public BillingPeriodUnit BillingPeriodUnit { get; set; }

    public string PoNumber { get; set; } = string.Empty;

    public string CustomerId { get; set; } = string.Empty;

    public SubscriptionStatus Status { get; set; }

    public long CurrentTermStart { get; set; }

    public long CurrentTermEnd { get; set; }

    public long NextBillingAt { get; set; }

    public long StartedAt { get; set; }

    public long ActivatedAt { get; set; }

    public bool HasScheduledChanges { get; set; }

    public string Channel { get; set; } = string.Empty;

    public long ResourceVersion { get; set; }

    public bool Deleted { get; set; }

    public string Object { get; set; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;

    public int DueInvoicesCount { get; set; }

    public int Mrr { get; set; }

    public bool OverrideRelationship { get; set; }

    public bool CreatePendingInvoices { get; set; }

    public bool AutoCloseInvoices { get; set; }


    #endregion
    #region foreign keys
    //[ForeignKey(nameof(TenantModel))]
    //public Guid? TenantId { get; set; }

    #endregion
    #region navigational properties
    //public TenantModel? Tenant { get; set; }
    public List<SubscriptionItemModel>? SubscriptionItems { get; set; }
    #endregion
}

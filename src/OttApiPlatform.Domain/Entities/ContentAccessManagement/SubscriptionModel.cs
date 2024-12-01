namespace OttApiPlatform.Domain.Entities.ContentAccessManagement;

[Table("Subscriptions")]
public class SubscriptionModel : BaseEntity
{
    public string Name { get; set; } = string.Empty;


    #region Public Properties

    public int BillingPeriod { get; set; }

    public SubscriptionStatus Status { get; set; }

    public BillingCycle Billing { get; set; }

    public TrialDays TrialDays { get; set; }

    public string PoNumber { get; set; } = string.Empty;

    public string CustomerId { get; set; } = string.Empty;


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
  
    #region navigational properties
   
    public List<SubscriptionItemModel> SubscriptionItems { get; set; }

    public ICollection<UserSubscriptionModel> UserSubscriptions { get; set; }
    #endregion
}

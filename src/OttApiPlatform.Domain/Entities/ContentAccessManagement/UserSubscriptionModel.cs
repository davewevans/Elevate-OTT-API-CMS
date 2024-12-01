using OttApiPlatform.Domain.Entities.Identity;

namespace OttApiPlatform.Domain.Entities.ContentAccessManagement;

[Table("UserSubscriptions")]
public class UserSubscriptionModel : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }

    public TransactionStatus PaymentStatus { get; set; }
  
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsRecurring { get; set; }

    public bool IsDeleted { get; set; } = false;

    public bool IsUpdated { get; set; } = false;

    public int Order { get; set; }


    #region Foreign Key Properties

    public string UserId { get; set; }

    public Guid SubscriptionId { get; set; } 

    #endregion

    #region Navigational Properties 

    public ApplicationUser User { get; set; }
    public SubscriptionModel Subscription { get; set; } 

    #endregion
}

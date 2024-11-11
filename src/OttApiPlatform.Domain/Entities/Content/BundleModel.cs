using OttApiPlatform.Domain.Enums.DirectSubscribers;

namespace OttApiPlatform.Domain.Entities.Content;

[Table("SubscriptionPlans")]

public class BundleModel : BaseEntity
{
    // freebie
    // rental
    // fixed price

    public BundleType BundleType { get; set; }
}

namespace OttApiPlatform.CMS.Enums;

public enum SubscriptionStatus
{
    Future, // The subscription is scheduled to start at a future date.
    InTrial, // The subscription is in trial.
    Active, // The subscription is active and will be charged for automatically based on the items in it.
    NotRenewing, // The subscription will be canceled at the end of the current term.
    Paused, // The subscription is paused. The subscription will not renew while in this state.
    Cancelled, // The subscription has been canceled and is no longer in service.
}

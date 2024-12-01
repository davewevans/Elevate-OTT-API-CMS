namespace OttApiPlatform.Domain.Enums;


public enum ContentAccessType
{
    /// <summary>
    /// Indicates that the content is free to access.
    /// </summary>
    Free,
    /// <summary>
    /// Indicates that the content is available for purchase.
    /// </summary>
    Purchase,
    /// <summary>
    /// Indicates that the content is available for rent.
    /// </summary>
    Rental,
    /// <summary>
    /// Indicates that the content is available for subscription.
    /// </summary>
    Subscription,

    /// <summary>
    /// Indicates that the content is available for pay-per-view.
    /// </summary>
    PPV
}

public enum RentalDuration
{
    /// <summary>
    /// Indicates the 1-day rental duration.
    /// </summary>
    OneDay,

    /// <summary>
    /// Indicates the 3-day rental duration.
    /// </summary>
    ThreeDays,

    /// <summary>
    /// Indicates the 7-day rental duration.
    /// </summary>
    SevenDays,

    /// <summary>
    /// Indicates the 2-week rental duration.
    /// </summary>
    TwoWeeks,

    /// <summary>
    /// Indicates the one-month rental duration.
    /// </summary>
    OneMonth,

    /// <summary>
    /// Indicates the three-month rental duration.
    /// </summary>
    ThreeMonths,

    /// <summary>
    /// Indicates the six-month rental duration.
    /// </summary>
    SixMonths,

    /// <summary>
    /// Indicates the one-year rental duration.
    /// </summary>
    OneYear
}

public enum AudienceType
{
    /// <summary>
    /// Indicates the general audience.
    /// </summary>
    General = 0,

    /// <summary>
    /// Indicates the kids audience.
    /// </summary>
    Kids = 1,

    /// <summary>
    /// Indicates the adult audience.
    /// </summary>
    Adult = 2
}

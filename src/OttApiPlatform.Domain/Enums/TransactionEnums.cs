namespace OttApiPlatform.Domain.Enums;

public enum PaymentGateway
{
    Stipe = 1,
    PayPal = 2,
}

public enum TransactionType
{
    Credit = 1,
    Debit = 2
}

public enum TransactionStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3
}

public enum TransactionCategory
{
    Subscription = 1,
    Purchase = 2,
    Refund = 3
}

public enum TransactionMethod
{
    Card = 1,
    Bank = 2,
    Cash = 3
}

public enum TransactionSource
{
    Web = 1,
    Mobile = 2,
    POS = 3
}

public enum TransactionCurrency
{
    USD = 1,
    EUR = 2,
    GBP = 3
}

public enum TransactionMode
{
    Live = 1,
    Test = 2
}

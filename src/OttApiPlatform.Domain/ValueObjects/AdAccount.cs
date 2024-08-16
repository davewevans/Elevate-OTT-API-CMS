namespace OttApiPlatform.Domain.ValueObjects;

public class AdAccount : ValueObject
{
    #region Private Constructors

    private AdAccount()
    {
    }

    #endregion Private Constructors

    #region Public Properties

    public string Domain { get; private set; }

    public string Name { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public static AdAccount For(string accountString)
    {
        var account = new AdAccount();

        try
        {
            var index = accountString.IndexOf("\\", StringComparison.Ordinal);
            account.Domain = accountString.Substring(0, index);
            account.Name = accountString.Substring(index + 1);
        }
        catch (Exception ex)
        {
            throw new AdAccountInvalidException(accountString, ex);
        }

        return account;
    }

    public static implicit operator string(AdAccount account)
    {
        return account.ToString();
    }

    public static explicit operator AdAccount(string accountString)
    {
        return For(accountString);
    }

    public override string ToString()
    {
        return $"{Domain}\\{Name}";
    }

    #endregion Public Methods

    #region Protected Methods

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Domain;
        yield return Name;
    }

    #endregion Protected Methods
}
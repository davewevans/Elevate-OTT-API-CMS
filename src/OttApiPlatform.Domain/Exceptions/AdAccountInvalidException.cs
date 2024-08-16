namespace OttApiPlatform.Domain.Exceptions;

public class AdAccountInvalidException : Exception
{
    #region Public Constructors

    public AdAccountInvalidException(string adAccount, Exception ex)
        : base($"AD Account \"{adAccount}\" is invalid.", ex)
    {
    }

    #endregion Public Constructors
}
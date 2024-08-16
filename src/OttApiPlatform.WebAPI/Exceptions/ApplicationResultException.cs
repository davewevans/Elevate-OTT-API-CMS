namespace OttApiPlatform.WebAPI.Exceptions;

public class ApplicationResultException : Exception
{
    #region Public Constructors

    public ApplicationResultException() : base("Internal server error")
    {
    }

    public ApplicationResultException(string message) : base(message)
    {
    }

    public ApplicationResultException(IDictionary<string, string> validationErrors)
    {
        ValidationErrors = new Dictionary<string, string>(validationErrors);
    }

    public ApplicationResultException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #endregion Public Constructors

    #region Public Properties

    public Dictionary<string, string> ValidationErrors { get; protected set; }

    #endregion Public Properties
}
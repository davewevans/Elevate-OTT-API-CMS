namespace OttApiPlatform.WebAPI.Exceptions;

public class ForbiddenAccessException : Exception
{
    #region Public Constructors

    public ForbiddenAccessException() : base("Forbidden access")
    {
    }

    public ForbiddenAccessException(string message) : base(message)
    {
    }

    public ForbiddenAccessException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #endregion Public Constructors
}
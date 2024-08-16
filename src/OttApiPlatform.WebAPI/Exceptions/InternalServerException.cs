namespace OttApiPlatform.WebAPI.Exceptions;

public class InternalServerException : Exception
{
    #region Public Constructors

    public InternalServerException() : base("Internal server error")
    {
    }

    public InternalServerException(string message) : base(message)
    {
    }

    public InternalServerException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #endregion Public Constructors
}
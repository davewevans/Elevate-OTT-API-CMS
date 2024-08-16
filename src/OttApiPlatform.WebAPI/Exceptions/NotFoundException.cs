namespace OttApiPlatform.WebAPI.Exceptions;

public class NotFoundException : Exception
{
    #region Public Constructors

    public NotFoundException() : base("Not found")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #endregion Public Constructors
}
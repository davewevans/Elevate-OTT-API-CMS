namespace OttApiPlatform.Application.Common.Exceptions;

public class FluentValidationException : ValidationException
{
    #region Public Constructors

    public FluentValidationException() : base("Model validation error(s)")
    {
    }

    public FluentValidationException(string message) : base(message)
    {
    }

    public FluentValidationException(IEnumerable<ValidationFailure> validationErrors) : base(validationErrors)
    {
    }

    #endregion Public Constructors
}
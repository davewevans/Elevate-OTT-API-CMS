namespace OttApiPlatform.Gateway.Models;

public class ApiErrorResponse
{
    #region Public Properties

    public Uri Type { get; set; }

    public string Title { get; set; }

    public int Status { get; set; }

    public string ErrorMessage { get; set; }

    public string Instance { get; set; }

    public List<ValidationError> ValidationErrors { get; set; } = new();

    #endregion Public Properties

    #region Public Classes

    public class ValidationError
    {
        #region Public Properties

        public string Name { get; set; }

        public string Reason { get; set; }

        #endregion Public Properties
    }

    #endregion Public Classes
}
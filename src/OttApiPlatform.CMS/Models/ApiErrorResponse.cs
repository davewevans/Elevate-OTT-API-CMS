namespace OttApiPlatform.CMS.Models;

public class ApiErrorResponse
{
    #region Public Properties

    public string Title { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
    public string Instance { get; set; }
    public string ErrorMessage { get; set; }
    public object InnerException { get; set; }

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
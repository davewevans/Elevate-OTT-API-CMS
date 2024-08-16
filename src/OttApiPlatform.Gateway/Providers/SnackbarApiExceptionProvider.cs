namespace OttApiPlatform.Gateway.Providers;

public class SnackbarApiExceptionProvider
{
    #region Private Fields

    private readonly ISnackbar _snackbar;

    #endregion Private Fields

    #region Public Constructors

    public SnackbarApiExceptionProvider(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    #endregion Public Constructors

    #region Public Methods

    public void ShowErrors(ApiErrorResponse apiErrorResponse)
    {
        if (!string.IsNullOrEmpty(apiErrorResponse.Title))
            _snackbar.Add(apiErrorResponse.Title, Severity.Info, (options) => options.CloseAfterNavigation = true);

        if (!string.IsNullOrEmpty(apiErrorResponse.ErrorMessage))
            _snackbar.Add(apiErrorResponse.ErrorMessage, Severity.Error, (options) => options.CloseAfterNavigation = true);

        if (apiErrorResponse.ValidationErrors != null && apiErrorResponse.ValidationErrors.Any())
            foreach (var error in apiErrorResponse.ValidationErrors)
                _snackbar.Add(error.Reason, Severity.Error, (options) => { options.CloseAfterNavigation = false; });
    }

    #endregion Public Methods
}
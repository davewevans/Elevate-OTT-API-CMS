namespace OttApiPlatform.CMS.Helpers;

public class EditContextApiExceptionFallback : ComponentBase
{
    #region Private Fields

    private ValidationMessageStore _messageStore;

    #endregion Private Fields

    #region Private Properties

    [CascadingParameter] private EditContext CurrentEditContext { get; set; }

    #endregion Private Properties

    #region Public Methods

    public void PopulateFormErrors(ApiErrorResponse apiErrorResponse)
    {
        _messageStore.Clear();

        if (apiErrorResponse.ValidationErrors != null)
        {
            foreach (var error in apiErrorResponse.ValidationErrors)
                _messageStore.Add(CurrentEditContext.Field(error.Name), error.Reason);
        }
        else
        {
            if (!string.IsNullOrEmpty(apiErrorResponse.ErrorMessage))
            {
                var fieldIdentifier = new FieldIdentifier(apiErrorResponse, $" {apiErrorResponse.ErrorMessage}");
                _messageStore.Add(fieldIdentifier, $"{apiErrorResponse.ErrorMessage}");
            }
            else
            {
                var fieldIdentifier = new FieldIdentifier(apiErrorResponse, $" {apiErrorResponse.Title}");
                _messageStore.Add(fieldIdentifier, $"{apiErrorResponse.Title}");
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    public void Invalidate()
    {
        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (s, e) => _messageStore.Clear();

        CurrentEditContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);

        CurrentEditContext.NotifyValidationStateChanged();
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (CurrentEditContext == null)
            throw new InvalidOperationException($"{nameof(EditContextApiExceptionFallback)} requires a cascading " +
                                                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(EditContextApiExceptionFallback)} " +
                                                "inside an EditForm.");
        Invalidate();
    }

    #endregion Protected Methods
}
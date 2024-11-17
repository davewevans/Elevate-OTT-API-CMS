namespace OttApiPlatform.CMS.Services;

public class SpinnerService
{
    #region Public Events

    public event Action OnShow;

    public event Action OnHide;

    #endregion Public Events

    #region Public Methods

    public void Show()
    {
        OnShow?.Invoke();
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }

    #endregion Public Methods
}
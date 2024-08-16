namespace OttApiPlatform.CMS.Helpers;

public class AppStateManager : IAppStateManager
{
    #region Private Fields

    private bool _playAudio;
    private bool _loaderOverlay;
    private bool _isCancellationRequested;

    #endregion Private Fields

    #region Public Events

    public event EventHandler PlayAudioChanged;

    public event EventHandler LoaderOverlayChanged;

    public event EventHandler TokenSourceChanged;

    #endregion Public Events

    #region Public Properties

    public bool PlayAudio
    {
        get => _playAudio;
        set
        {
            _playAudio = value;
            PlayAudioChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool OverlayVisible
    {
        get => _loaderOverlay;
        set
        {
            _loaderOverlay = value;
            LoaderOverlayChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsCancellationRequested
    {
        get => _isCancellationRequested;
        set
        {
            _isCancellationRequested = value;
            TokenSourceChanged?.Invoke(this, EventArgs.Empty);
            OverlayVisible = false;
        }
    }

    public string UserPasswordFor2Fa { get; set; }

    #endregion Public Properties
}
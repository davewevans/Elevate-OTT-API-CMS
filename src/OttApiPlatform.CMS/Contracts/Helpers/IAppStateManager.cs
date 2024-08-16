namespace OttApiPlatform.CMS.Contracts.Helpers;

public interface IAppStateManager
{
    #region Public Events

    event EventHandler PlayAudioChanged;

    event EventHandler LoaderOverlayChanged;

    event EventHandler TokenSourceChanged;

    #endregion Public Events

    #region Public Properties

    public bool PlayAudio { get; set; }
    public bool OverlayVisible { get; set; }
    bool IsCancellationRequested { get; set; }
    string UserPasswordFor2Fa { get; set; }

    #endregion Public Properties
}
namespace OttApiPlatform.WebAPI.Managers;

public class TimerManager
{
    #region Private Fields

    private Timer _timer;
    private AutoResetEvent _autoResetEvent;
    private Action _action;

    #endregion Private Fields

    #region Public Properties

    public DateTime TimerStarted { get; set; }

    public bool IsTimerStarted { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void PrepareTimer(Action action)
    {
        _action = action;
        _autoResetEvent = new AutoResetEvent(false);
        _timer = new Timer(Execute, _autoResetEvent, 1000, 2000);
        TimerStarted = DateTime.Now;
        IsTimerStarted = true;
    }

    /// <summary>
    /// Execute timer service.
    /// </summary>
    /// <param name="stateInfo"></param>
    public void Execute(object stateInfo)
    {
        // Execute the action associated with the timer.
        _action();

        // If the timer has been running for less than 120 seconds, return.
        if (!((DateTime.Now - TimerStarted).TotalSeconds > 120))
            return;

        // Set IsTimerStarted to false since the timer has stopped.
        IsTimerStarted = false;

        // Dispose of the timer.
        _timer.Dispose();
    }

    #endregion Public Methods
}
namespace OttApiPlatform.WebAPI.Hubs;

[Authorize]
public class DataExportHub : Hub
{
    #region Private Fields

    private readonly IBackgroundJobClient _backgroundJob;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISignalRContextProvider _signalRContextProvider;
    private readonly IBackgroundReportingService _backgroundReportingService;

    #endregion Private Fields

    #region Public Constructors

    public DataExportHub(IBackgroundJobClient backgroundJob,
                         IHttpContextAccessor httpContextAccessor,
                         ISignalRContextProvider signalRContextProvider,
                         IBackgroundReportingService backgroundReportingService)
    {
        _backgroundJob = backgroundJob;
        _httpContextAccessor = httpContextAccessor;
        _signalRContextProvider = signalRContextProvider;
        _backgroundReportingService = backgroundReportingService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task ExportApplicantToPdf(ExportApplicantsQuery request)
    {
        // Get the name of the user who initiated the request.
        var issuerName = _signalRContextProvider.GetUserName(Context);

        // Set the tenant id based on the request context.
        _signalRContextProvider.SetTenantIdViaTenantResolver(Context);

        // Get the tenant id.
        var tenantId = _signalRContextProvider.GetTenantId(Context);

        // Get the unique identifier of the user who initiated the request.
        var userNameIdentifier = _signalRContextProvider.GetUserNameIdentifier(Context);

        // Get the HTTP request associated with the context.
        var httpRequest = Context.GetHttpContext()?.Request;

        if (httpRequest == null)
            throw new NullReferenceException(nameof(httpRequest));

        // Get the host name associated with the context.
        var baseUrl= _signalRContextProvider.GetHostName(Context);

        // Generate a unique report id.
        var reportId = Guid.NewGuid();

        // TODO: Uncomment the following code block if Hangfire is not being used.
        //await _backgroundReportingService.InitiateApplicantsReport(request, reportId, userNameIdentifier);
        //await _backgroundReportingService.ExportApplicantsAsPdfInBackground(request, reportId, host, issuerName, userNameIdentifier);

        var pendingJob = _backgroundJob.Enqueue(() => _backgroundReportingService.InitiateApplicantsReport(request, reportId, userNameIdentifier, tenantId));

        // Add a record to the Reports Table with an in-progress status.
        _backgroundJob.ContinueJobWith(pendingJob, () => _backgroundReportingService.ExportApplicantsAsPdfInBackground(request,
                                                                                                                       reportId,
                                                                                                                       baseUrl,
                                                                                                                       userNameIdentifier,
                                                                                                                       tenantId));

        await Task.CompletedTask;
    }

    public override Task OnConnectedAsync()
    {
        if (Context.UserIdentifier != null)
        {
            // Get the name of the user who connected to the hub.
            var name = _signalRContextProvider.GetUserName(Context);

            // Add the user to the group associated with their name.
            Groups.AddToGroupAsync(Context.ConnectionId, name);
        }

        return base.OnConnectedAsync();
    }

    #endregion Public Methods
}
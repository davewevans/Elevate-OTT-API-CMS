namespace OttApiPlatform.Application.Common.Behaviours;

/// <summary>
/// Represents a pipeline behavior for handling unhandled exceptions in a request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    #region Private Fields

    private readonly ILogger<TRequest> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUtcTimeService _timeService;

    #endregion Private Fields

    #region Public Constructors

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor, IUtcTimeService timeService)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _timeService = timeService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            // Call the next handler in the pipeline and return its response.
            return await next();
        }
        catch (Exception ex)
        {
            // Get the name of the request type.
            var requestName = typeof(TRequest).Name;

            // Get the name of the response type.
            var responseName = typeof(TResponse).Name;

            // Get the ID of the user making the request.
            var userId = _httpContextAccessor.GetUserId();

            // Log the exception.
            _logger.LogError(ex, "Request Info: Request name: {@RequestName} | Request path: {requestPath} | Response name: {ResponseName} | Requested by: {@UserId} | Requested on: {@RequestOn}",
                             requestName,
                             request,
                             responseName,
                             userId,
                             _timeService.GetUtcNow());

            // Rethrow the exception with the original message and include the error string.
            throw new Exception($"An unhandled exception has occurred while executing the {requestName}: {ex.Message}", ex.InnerException);
        }
    }

    #endregion Public Methods
}
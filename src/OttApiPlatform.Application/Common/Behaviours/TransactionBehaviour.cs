namespace OttApiPlatform.Application.Common.Behaviours;

/// <summary>
/// Represents a pipeline behavior for managing transactions in a request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    #region Private Fields

    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
    private readonly IApplicationDbContext _dbContext;

    #endregion Private Fields

    #region Public Constructors

    public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, IApplicationDbContext dbContext)
    {
        _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        _dbContext = dbContext;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Create a default response object
        var response = default(TResponse);

        // Get the name of the request type
        var typeName = request.GetType().Name;

        // If the request is not a command, do not use a transaction.
        if (!typeName.EndsWith("Command"))
            // Call the next handler in the pipeline and return its response.
            return await next();

        // Create an execution strategy to handle any transient exceptions.
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.MaximumTimeout
        };

        // Start a new transaction scope.
        using var scope = new TransactionScope(TransactionScopeOption.Required,
                                               transactionOptions,
                                               TransactionScopeAsyncFlowOption.Enabled);

        // Use the execution strategy to execute the transactional code.
        await strategy.ExecuteAsync(async () =>
                                    {
                                        // Log that the transaction has begun.
                                        _logger.LogInformation($"Begin Transaction: {typeof(TRequest).Name}");

                                        // Call the next handler in the pipeline and get its response.
                                        response = await next();

                                        // Get the value of the "IsError" property from the
                                        // response, default to false if property does not exist.
                                        var isError = response?.GetType().GetProperty("IsError")?.GetValue(response, null) as bool? ?? false;

                                        // Get the value of the "RollbackDisabled" property from the
                                        // response, default to false if property does not exist.
                                        var rollbackDisabled = response?.GetType().GetProperty("RollbackDisabled")?.GetValue(response, null) as bool? ?? false;

                                        // If the request pipeline completed successfully or the
                                        // RollbackDisabled flag is set to true, complete the transaction.
                                        if (isError is false || rollbackDisabled)
                                        {
                                            scope.Complete();
                                            _logger.LogInformation($"Committed Transaction: {typeof(TRequest).Name}");
                                        }
                                    });

        // Return the response.
        return response;
    }

    #endregion Public Methods
}
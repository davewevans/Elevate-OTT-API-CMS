namespace OttApiPlatform.Application;

public static class DependencyInjection
{
    #region Public Methods

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Adds MediatR to the dependency injection container and configures it to scan the current
        // assembly for services.
        services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                                config.NotificationPublisher = new TaskWhenAllPublisher();
                            });

        // Adds a transient service for PerformanceBehaviour to the dependency injection container.
        // This is used as a pipeline behavior for MediatR requests.
        // TODO: Uncomment to if you want to measure the request performance.
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        // Adds a transient service for LoggingBehaviour to the dependency injection container.
        // This is used as a pipeline behavior for MediatR requests.
        // TODO: Uncomment to if you want to log the request info.
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        // Adds a transient service for ValidationBehavior to the dependency injection container.
        // This is used as a pipeline behavior for MediatR requests.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Adds a transient service for TransactionBehaviour to the dependency injection container.
        // This is used as a pipeline behavior for MediatR requests.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

        // Adds a transient service for UnhandledExceptionBehaviour to the dependency injection
        // container. This is used as a pipeline behavior for MediatR requests.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

        // Returns the service collection.
        return services;
    }

    #endregion Public Methods
}
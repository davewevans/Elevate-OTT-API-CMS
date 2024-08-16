namespace OttApiPlatform.WebAPI.Middleware;

public static class BpExceptionHandlerMiddleware
{
    #region Public Methods

    /// <summary>
    /// Adds a global exception handler middleware to the pipeline that handles exceptions thrown
    /// from the application and sends a properly formatted response to the client.
    /// </summary>
    /// <param name="app">The IApplicationBuilder instance.</param>
    /// <param name="loggingEnabled">A boolean indicating whether logging is enabled or not.</param>
    /// <param name="debuggingEnabled">A boolean indicating whether debugging is enabled or not.</param>
    public static void UseBpExceptionHandler(this IApplicationBuilder app, bool loggingEnabled, bool debuggingEnabled)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (exceptionHandlerFeature != null)
                {
                    var loggerFactory = context.RequestServices.GetService<ILoggerFactory>();

                    var logger = loggerFactory?.CreateLogger("BpExceptionHandlerMiddleware");

                    if (loggingEnabled && logger != null)
                        logger.LogError(exceptionHandlerFeature.Error, "Unhandled Exception");

                    var apiErrorResponse = new ApiErrorResponse
                    {
                        Instance = context.Request.Path,
                    };

                    switch (exceptionHandlerFeature.Error)
                    {
                        case FluentValidationException fluentValidationException:
                            apiErrorResponse.Type = "https://httpstatuses.com/422";
                            apiErrorResponse.Title = Resource.Model_validation_errors_occurred_while_processing_your_request;
                            apiErrorResponse.Status = HttpStatusCode.UnprocessableEntity;
                            apiErrorResponse.ValidationErrors = fluentValidationException.Errors.Select(failure => new ValidationError
                            {
                                Name = failure.PropertyName,
                                Reason = failure.ErrorMessage
                            }).ToList();
                            break;

                        case ApplicationResultException exception:
                            apiErrorResponse.Type = "https://httpstatuses.com/422";
                            apiErrorResponse.Title = Resource.Application_errors_occurred_while_processing_your_request;
                            apiErrorResponse.Status = HttpStatusCode.UnprocessableEntity;
                            apiErrorResponse.ValidationErrors = exception.ValidationErrors.Select(failure => new ValidationError
                            {
                                Name = failure.Key,
                                Reason = failure.Value
                            }).ToList();
                            break;

                        case UnauthorizedAccessException exception:
                            apiErrorResponse.Type = "https://httpstatuses.com/401";
                            apiErrorResponse.Title = exception.Message;
                            apiErrorResponse.Status = HttpStatusCode.Unauthorized;
                            break;

                        case ForbiddenAccessException exception:
                            apiErrorResponse.Type = "https://httpstatuses.com/403";
                            apiErrorResponse.Title = exception.Message;
                            apiErrorResponse.Status = HttpStatusCode.Forbidden;
                            break;

                        case BadRequestException:
                            apiErrorResponse.Type = "https://httpstatuses.com/400";
                            apiErrorResponse.Title = Resource.The_request_was_not_processed_due_to_invalid_or_missing_parameters;
                            apiErrorResponse.Status = HttpStatusCode.BadRequest;
                            apiErrorResponse.ErrorMessage = exceptionHandlerFeature.Error.Message;
                            apiErrorResponse.InnerException = debuggingEnabled ? exceptionHandlerFeature.Error.InnerException : string.Empty;
                            apiErrorResponse.StackTrace = debuggingEnabled ? exceptionHandlerFeature.Error.StackTrace : string.Empty;
                            break;

                        case NotFoundException:
                            apiErrorResponse.Type = "https://httpstatuses.com/404";
                            apiErrorResponse.Title = Resource.The_requested_resource_was_not_found;
                            apiErrorResponse.Status = HttpStatusCode.NotFound;
                            apiErrorResponse.ErrorMessage = exceptionHandlerFeature.Error.Message;
                            apiErrorResponse.InnerException = debuggingEnabled ? exceptionHandlerFeature.Error.InnerException : string.Empty;
                            apiErrorResponse.StackTrace = debuggingEnabled ? exceptionHandlerFeature.Error.StackTrace : string.Empty;
                            break;

                        case InternalServerException:
                            apiErrorResponse.Type = "https://httpstatuses.com/500";
                            apiErrorResponse.Title = Resource.An_internal_server_error_occurred_while_processing_your_request;
                            apiErrorResponse.Status = HttpStatusCode.InternalServerError;
                            apiErrorResponse.ErrorMessage = exceptionHandlerFeature.Error.Message;
                            apiErrorResponse.InnerException = debuggingEnabled ? exceptionHandlerFeature.Error.InnerException : string.Empty;
                            apiErrorResponse.StackTrace = debuggingEnabled ? exceptionHandlerFeature.Error.StackTrace : string.Empty;
                            break;

                        default:
                            apiErrorResponse.Type = "https://httpstatuses.com/500";
                            apiErrorResponse.Title = Resource.An_error_occurred_while_processing_your_request;
                            apiErrorResponse.Status = HttpStatusCode.InternalServerError;
                            apiErrorResponse.ErrorMessage = exceptionHandlerFeature.Error.Message;
                            apiErrorResponse.InnerException = debuggingEnabled ? exceptionHandlerFeature.Error.InnerException : string.Empty;
                            apiErrorResponse.StackTrace = debuggingEnabled ? exceptionHandlerFeature.Error.StackTrace : string.Empty;
                            break;
                    }

                    context.Response.ContentType = "application/json";

                    context.Response.StatusCode = (int)apiErrorResponse.Status;

                    var json = JsonSerializer.Serialize(apiErrorResponse);

                    await context.Response.WriteAsync(json);
                }
            });
        });
    }

    #endregion Public Methods
}
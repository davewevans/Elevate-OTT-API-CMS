namespace OttApiPlatform.WebAPI.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    #region Private Fields

    private IMediator _mediator;

    #endregion Private Fields

    #region Protected Properties

    /// <summary>
    /// A protected property to access the instance of IMediator.
    /// </summary>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    #endregion Protected Properties

    #region Protected Methods

    /// <summary>
    /// Tries to get the result from the specified envelope.
    /// </summary>
    /// <typeparam name="T">The type of the payload in the envelope.</typeparam>
    /// <param name="envelope">The envelope containing the payload.</param>
    /// <returns>An IActionResult representing the result.</returns>
    protected IActionResult TryGetResult<T>(Envelope<T> envelope)
    {
        ThrowExceptionsIfExist(envelope);

        var apiResponse = new ApiSuccessResponse<T>(envelope, HttpContext.Request.Path);

        return Ok(apiResponse);
    }

    #endregion Protected Methods

    #region Private Methods

    private static void ThrowExceptionsIfExist(EnvelopeBase envelope)
    {
        switch (envelope.IsError)
        {
            case true when envelope.ValidationErrors.Any():
                throw new ApplicationResultException(envelope.ValidationErrors);
            case true:
                throw envelope.HttpStatusCode switch
                {
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException(envelope.Title),
                    HttpStatusCode.Forbidden => new ForbiddenAccessException(envelope.Title),
                    HttpStatusCode.NotFound => new NotFoundException(envelope.Title),
                    HttpStatusCode.BadRequest => new BadRequestException(envelope.Title),
                    _ => new InternalServerException(envelope.Title)
                };
        }
    }

    #endregion Private Methods
}
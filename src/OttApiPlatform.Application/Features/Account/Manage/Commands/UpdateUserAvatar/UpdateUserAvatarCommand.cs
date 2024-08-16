namespace OttApiPlatform.Application.Features.Account.Manage.Commands.UpdateUserAvatar;

public class UpdateUserAvatarCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string AvatarUri { get; set; }
    public bool IsAvatarAdded { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class UpdateUserAvatarCommandHandler : IRequestHandler<UpdateUserAvatarCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public UpdateUserAvatarCommandHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Return an error response if the user ID is invalid or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user with the given ID.
            var user = await _userManager.FindByIdAsync(userId);

            // Return an error response if the user cannot be found.
            if (user == null)
                return Envelope<string>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Update the user's avatar URI.
            user.AvatarUri = request.AvatarUri;

            // Update the user in the database and return an appropriate response.
            var identityResult = await _userManager.UpdateAsync(user);

            return !identityResult.Succeeded
                ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError)
                : Envelope<string>.Result.Ok(Resource.User_has_been_updated_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
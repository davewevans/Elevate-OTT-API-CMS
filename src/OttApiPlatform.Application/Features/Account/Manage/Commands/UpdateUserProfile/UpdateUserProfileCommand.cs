namespace OttApiPlatform.Application.Features.Account.Manage.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Name { get; set; }
    public string Surname { get; set; }
    public string JobTitle { get; set; }
    public string PhoneNumber { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(ApplicationUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.Name = Name;
        user.Surname = Surname;
        user.JobTitle = JobTitle;
        user.PhoneNumber = PhoneNumber;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public UpdateUserProfileCommandHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
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

            // Map the request properties to the user entity.
            request.MapToEntity(user);

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
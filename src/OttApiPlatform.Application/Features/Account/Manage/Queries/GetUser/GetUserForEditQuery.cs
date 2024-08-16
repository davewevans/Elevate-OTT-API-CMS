namespace OttApiPlatform.Application.Features.Account.Manage.Queries.GetUser;

public class GetCurrentUserForEditQuery : IRequest<Envelope<CurrentUserForEditResponse>>
{
    #region Public Classes

    public class GetCurrentUserForEditQueryHandler : IRequestHandler<GetCurrentUserForEditQuery, Envelope<CurrentUserForEditResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public GetCurrentUserForEditQueryHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<CurrentUserForEditResponse>> Handle(GetCurrentUserForEditQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Return BadRequest if user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<CurrentUserForEditResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await _userManager.FindByIdAsync(userId);

            // Return Unauthorized if user is null.
            if (user == null)
                return Envelope<CurrentUserForEditResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Create a CurrentUserForEditResponse object with the user's information.
            var currentUserForEditResponse = new CurrentUserForEditResponse
            {
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                JobTitle = user.JobTitle,
                AvatarUri = user.AvatarUri
            };

            // Return Ok with the CurrentUserForEditResponse object.
            return Envelope<CurrentUserForEditResponse>.Result.Ok(currentUserForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
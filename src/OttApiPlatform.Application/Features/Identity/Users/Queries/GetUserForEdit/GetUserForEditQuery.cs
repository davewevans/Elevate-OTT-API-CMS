namespace OttApiPlatform.Application.Features.Identity.Users.Queries.GetUserForEdit;

public class GetUserForEditQuery : IRequest<Envelope<UserForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetUserForEditQueryHandler : IRequestHandler<GetUserForEditQuery, Envelope<UserForEditResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;

        #endregion Private Fields

        #region Public Constructors

        public GetUserForEditQueryHandler(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<UserForEditResponse>> Handle(GetUserForEditQuery request, CancellationToken cancellationToken)
        {
            // Check if request Id is null or white space and return BadRequest result if true.
            if (string.IsNullOrWhiteSpace(request.Id))
                return Envelope<UserForEditResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Get the user entity from the database, including related entities for user roles and
            // user attachments.
            var user = await _userManager.Users.Include(u => u.UserRoles)
                                               .ThenInclude(ur => ur.Role)
                                               .Include(u => u.UserAttachments)
                                               .Where(u => u.Id == request.Id)
                                               .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return NotFound result if user entity is null.
            if (user == null)
                return Envelope<UserForEditResponse>.Result.NotFound(Resource.Unable_to_load_user);

            // Map the assigned user roles and attachments to view models.
            var assignedRoles = user.UserRoles.Select(AssignedUserRoleItem.MapFromEntity).ToList();
            var assignedAttachments = user.UserAttachments.Select(AssignedUserAttachmentItem.MapFromEntity).ToList();

            // Map the user entity to the UserForEditResponse view model and return an Ok result.
            var userForEditResponse = UserForEditResponse.MapFromEntity(user, assignedRoles, assignedAttachments);
            return Envelope<UserForEditResponse>.Result.Ok(userForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
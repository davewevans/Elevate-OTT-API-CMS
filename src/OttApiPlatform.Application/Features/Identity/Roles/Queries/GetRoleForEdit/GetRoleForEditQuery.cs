namespace OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRoleForEdit;

public class GetRoleForEditQuery : IRequest<Envelope<RoleForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetRoleForEditQueryHandler : IRequestHandler<GetRoleForEditQuery, Envelope<RoleForEditResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public GetRoleForEditQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<RoleForEditResponse>> Handle(GetRoleForEditQuery request, CancellationToken cancellationToken)
        {
            // Checks if the Id provided in the request is valid.
            if (string.IsNullOrWhiteSpace(request.Id))
                return Envelope<RoleForEditResponse>.Result.BadRequest(Resource.Invalid_role_Id);

            // Retrieves the role from the database.
            var role = await _dbContext.Roles.Include(r => r.RoleClaims).Where(r => r.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Checks if the role was found.
            if (role == null)
                return Envelope<RoleForEditResponse>.Result.NotFound(Resource.Unable_to_load_role);

            // Maps the role to a RoleForEditResponse object.
            var roleForEditResponse = RoleForEditResponse.MapFromEntity(role);

            // Returns the mapped role in an Envelope object with the Ok status.
            return Envelope<RoleForEditResponse>.Result.Ok(roleForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
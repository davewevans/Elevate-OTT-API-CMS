namespace OttApiPlatform.Application.Features.Identity.Roles.Commands.UpdateRole;

public class UpdateRoleCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public IReadOnlyList<Guid> SelectedPermissionIds { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(ApplicationRole role)
    {
        if (role == null)
            throw new ArgumentNullException(nameof(role));

        role.Name = Name;
        role.IsDefault = IsDefault;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ApplicationRoleManager _roleManager;
        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public UpdateRoleCommandHandler(ApplicationRoleManager roleManager, IApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            // Check if the role ID is valid.
            if (string.IsNullOrEmpty(request.Id))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_role_Id);

            // Get the role to update from the role manager.
            var role = await _roleManager.Roles.Include(r => r.RoleClaims).Where(r => r.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the role is not found, return an error message.
            if (role == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_role);

            // If the role is static, return an error message.
            if (role.IsStatic)
                return Envelope<string>.Result.ServerError(Resource.Unable_to_update_static_role);

            // Update the role with the new values from the command.
            request.MapToEntity(role);

            // Add or remove role permissions based on the selected permission IDs.
            await _roleManager.AddOrRemoveRolePermission(request.SelectedPermissionIds, role);

            // Update the role in the role manager.
            var identityResult = await _roleManager.UpdateAsync(role);

            // Save changes to the database.
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Return an error message if the update failed, or a success message if it succeeded.
            return !identityResult.Succeeded
                ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError)
                : Envelope<string>.Result.Ok(Resource.Role_has_been_updated_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
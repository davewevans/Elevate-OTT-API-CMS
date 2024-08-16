namespace OttApiPlatform.Application.Features.Identity.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public bool SetRandomPassword { get; set; }
    public string AvatarUri { get; set; }
    public bool IsAvatarAdded { get; set; }
    public int NumberOfAttachments { get; set; } = 0;
    public string Name { get; set; }
    public string Surname { get; set; }
    public string JobTitle { get; set; }
    public string PhoneNumber { get; set; }
    public bool MustSendActivationEmail { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsSuspended { get; set; }

    public IReadOnlyList<string> AssignedRoleIds { get; set; }
    public IReadOnlyList<string> AttachmentIds { get; set; }
    public IReadOnlyList<string> AttachmentUris { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(ApplicationUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.UserName = Email;
        user.Email = Email;
        user.Name = Name;
        user.Surname = Surname;
        user.JobTitle = JobTitle;
        user.AvatarUri = AvatarUri;
        user.PhoneNumber = PhoneNumber;
        user.IsSuperAdmin = IsSuperAdmin;
        user.IsSuspended = IsSuspended;

        AddOrRemoveUserAttachments(user);
    }

    #endregion Public Methods

    #region Private Methods

    private void AddOrRemoveUserAttachments(ApplicationUser user)
    {
        if (AttachmentIds is not null)
        {
            var removedUserAttachments = user.UserAttachments.Where(ua => AttachmentIds.All(aid => aid != ua.Id.ToString())).ToList();

            foreach (var userAttachment in removedUserAttachments)
                user.UserAttachments.Remove(userAttachment);
        }

        if (AttachmentUris?.Any() == true)
            foreach (var attachmentUri in AttachmentUris)
                user.UserAttachments.Add(new ApplicationUserAttachment
                {
                    FileUri = attachmentUri,
                    FileName = attachmentUri.Split("/").Last(),
                    UserId = user.Id,
                });
    }

    #endregion Private Methods

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;

        #endregion Private Fields

        #region Public Constructors

        public UpdateUserCommandHandler(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the user with the specified Id and include related entities such as user
            // roles and claims.
            var user = await _userManager.Users.Include(u => u.UserRoles)
                                               .ThenInclude(r => r.Role)
                                               .ThenInclude(r => r.RoleClaims)
                                               .Include(u => u.UserAttachments)
                                               .Include(u => u.Claims)
                                               .Where(u => u.Id == request.Id)
                                               .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If user is not found, return a not found error response.
            if (user == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_user);

            // Map the request to the user entity.
            request.MapToEntity(user);

            // TODO: Uncomment this section if you want to enable generating user password.
            //if (request.SetRandomPassword && user.EmailConfirmed)
            //    request.Password = _userManager.GenerateRandomPassword(6);

            // TODO: Uncomment this section if you want to enable resetting user password.
            //if (!string.IsNullOrWhiteSpace(request.Password))
            //{
            //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //    var response = await _userManager.ResetPasswordAsync(request.Password, user, token);
            //    if (response.IsError)
            //        return response;
            //}

            // If the user is not a static type, add or remove user roles based on the assigned role Ids.
            if (!user.IsStatic)
                _userManager.AddOrRemoveUserRoles(request.AssignedRoleIds, user);

            // If the request requires sending an activation email and the user email is not
            // confirmed,// send the email.
            if (request.MustSendActivationEmail && !user.EmailConfirmed)
                await _userManager.SendActivationEmailAsync(user);

            // TODO: Uncomment this section if you want to enable resetting user password to the default password.
            //if (!string.IsNullOrWhiteSpace(updateUserCommand.Password))
            //    await _demoUserPasswordSetterService.ResetDefaultPassword(updateUserCommand.Password, user);

            // Update the user using the user manager.
            var identityResult = await _userManager.UpdateAsync(user);

            // If the user is static, return a response without updating the user roles.
            if (user.IsStatic)
                return !identityResult.Succeeded
                    ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError)
                    : Envelope<string>.Result.Ok(Resource.User_has_been_updated_successfully_without_updating_his_her_roles_as_the_user_is_static_type);

            // Otherwise, return a response with or without errors based on the success of the
            // identity result.
            return !identityResult.Succeeded
                ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError)
                : Envelope<string>.Result.Ok(Resource.User_has_been_updated_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Constructors
}
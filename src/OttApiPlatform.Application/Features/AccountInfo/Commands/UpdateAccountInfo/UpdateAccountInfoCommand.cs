namespace OttApiPlatform.Application.Features.AccountInfo.Commands.UpdateAccountInfo;

public class UpdateAccountInfoCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string ChannelName { get; set; }
    public string LicenseKey { get; set; }
    public string SubDomain { get; set; }
    public string CustomDomain { get; set; }
    public string StorageFileNamePrefix { get; set; }
    public Guid TenantId { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class UpdateAccountInfoCommandHandler : IRequestHandler<UpdateAccountInfoCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly ApplicationUserManager _userManager;

        #endregion Private Fields

        #region Public Constructors

        public UpdateAccountInfoCommandHandler(IApplicationDbContext dbContext, ApplicationUserManager userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateAccountInfoCommand request, CancellationToken cancellationToken)
        {
            var accountInfo = await _dbContext.AccountInfo.FindAsync(request.TenantId);
            if (accountInfo == null)
            {
                return Envelope<string>.Result.BadRequest("Account info not found");
            }

            accountInfo.ChannelName = request.ChannelName;
            accountInfo.LicenseKey = request.LicenseKey;
            accountInfo.SubDomain = request.SubDomain;
            accountInfo.CustomDomain = request.CustomDomain;
            accountInfo.StorageFileNamePrefix = request.StorageFileNamePrefix;
            accountInfo.ModifiedBy = request.ModifiedBy;
            accountInfo.ModifiedOn = request.ModifiedOn;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Envelope<string>.Result;
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}

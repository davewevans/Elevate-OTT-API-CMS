using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Features.AccountInfo.Commands.CreateAccountInfo;
public class CreateAccountInfoCommand : IRequest<Envelope<CreateAccountInfoResponse>>
{
    #region Public Properties

    public string ChannelName { get; set; }
    public string LicenseKey { get; set; }
    public string SubDomain { get; set; }
    public string CustomDomain { get; set; }
    public string StorageFileNamePrefix { get; set; }
    public Guid TenantId { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class CreateAccountInfoCommandHandler : IRequestHandler<CreateAccountInfoCommand, Envelope<CreateAccountInfoResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _context;

        #endregion Private Fields

        #region Public Constructors

        public CreateAccountInfoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<CreateAccountInfoResponse>> Handle(CreateAccountInfoCommand request, CancellationToken cancellationToken)
        {
            var accountInfo = new Domain.Entities.AccountInfoModel
            {
                ChannelName = request.ChannelName,
                LicenseKey = request.LicenseKey,
                SubDomain = request.SubDomain,
                CustomDomain = request.CustomDomain,
                StorageFileNamePrefix = request.StorageFileNamePrefix,
                TenantId = request.TenantId
            };

            _context.AccountInfo.Add(accountInfo);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateAccountInfoResponse
            {
                Id = accountInfo.Id.ToString(),
                SuccessMessage = "Account information created successfully."
            };

            return Envelope<CreateAccountInfoResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}

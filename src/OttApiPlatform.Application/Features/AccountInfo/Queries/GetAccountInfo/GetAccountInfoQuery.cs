using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OttApiPlatform.Domain.Entities;

namespace OttApiPlatform.Application.Features.AccountInfo.Queries.GetAccountInfo;

public class GetAccountInfoQuery : IRequest<Envelope<AccountInfoResponse>>
{
    #region Public Properties

    public Guid TenantId { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetAccountInfoQueryHandler : IRequestHandler<GetAccountInfoQuery, Envelope<AccountInfoResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;

        #endregion Private Fields

        #region Public Constructors

        public GetAccountInfoQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<AccountInfoResponse>> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
        {
            var accountInfo = await _dbContext.AccountInfo.FindAsync(new object[] { request.TenantId, cancellationToken }, cancellationToken: cancellationToken);
            if (accountInfo == null)
            {
                return Envelope<AccountInfoResponse>.Result.BadRequest("Account info not found");
            }

            var accountInfoResponse = AccountInfoResponse.MapFromEntity(accountInfo);

            return Envelope<AccountInfoResponse>.Result.Ok(accountInfoResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}

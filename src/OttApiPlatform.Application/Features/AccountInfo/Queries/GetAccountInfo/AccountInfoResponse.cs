using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Features.AccountInfo.Queries.GetAccountInfo;
public class AccountInfoResponse
{
    #region Public Properties

    public string CompanyName { get; set; }
    public string LicenseKey { get; set; }
    public string SubDomain { get; set; }
    public string CustomDomain { get; set; }
    public string StorageFileNamePrefix { get; set; }
    public Guid TenantId { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static AccountInfoResponse MapFromEntity(Domain.Entities.AccountInfo accountInfo)
    {
        return new AccountInfoResponse
        {
            CompanyName = accountInfo.CompanyName,
            LicenseKey = accountInfo.LicenseKey,
            SubDomain = accountInfo.SubDomain,
            CustomDomain = accountInfo.CustomDomain,
            StorageFileNamePrefix = accountInfo.StorageFileNamePrefix,
            TenantId = accountInfo.TenantId,
            CreatedBy = accountInfo.CreatedBy,
            CreatedOn = accountInfo.CreatedOn,
            ModifiedBy = accountInfo.ModifiedBy,
            ModifiedOn = accountInfo.ModifiedOn,
            DeletedBy = accountInfo.DeletedBy,
            DeletedOn = accountInfo.DeletedOn
        };
    }

    #endregion Public Methods
}

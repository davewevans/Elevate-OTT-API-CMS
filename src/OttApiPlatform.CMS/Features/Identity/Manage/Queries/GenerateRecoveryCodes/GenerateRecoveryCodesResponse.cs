namespace OttApiPlatform.CMS.Features.Identity.Manage.Queries.GenerateRecoveryCodes;

public class GenerateRecoveryCodesResponse
{
    #region Public Properties

    public IEnumerable<string> RecoveryCodes { get; set; }
    public string StatusMessage { get; set; }

    #endregion Public Properties
}
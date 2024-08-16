namespace OttApiPlatform.Application.Features.Account.Manage.Queries.GenerateRecoveryCodes;

public class GenerateRecoveryCodesResponse
{
    #region Public Constructors

    public GenerateRecoveryCodesResponse()
    {
        RecoveryCodes = new List<string>();
    }

    #endregion Public Constructors

    #region Public Properties

    public IEnumerable<string> RecoveryCodes { get; set; }
    public string StatusMessage { get; set; }

    #endregion Public Properties
}
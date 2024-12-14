namespace OttApiPlatform.Domain.Entities.Mux;


[Table("MuxSettings")]
public class MuxSettingsModel : BaseEntity, IMustHaveTenant
{
    #region Public Properties
  
    public Guid TenantId { get; set; }
    public string TokenId { get; set; }
    public string TokenSecret { get; set; }
    public string SigningSecret { get; set; }
    public string SigningKeyId { get; set; }
    public string PrivateKey { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string CorsOrigin { get; set; }
    public string RtmpUrl { get; set; }
    public string RtmpsUrl { get; set; }
    public string Environment { get; set; }
    public string EnvironmentId { get; set; }

    #endregion
}

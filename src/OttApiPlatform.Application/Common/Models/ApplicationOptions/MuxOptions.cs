namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

public class MuxOptions
{
    #region Public Fields

    public const string Section = "Mux";

    #endregion Public Fields

    #region Public Properties

    public string SigningSecret { get; set; } = string.Empty;
    public string TokenId { get; set; } = string.Empty;
    public string TokenSecret { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string AssetPath { get; set; } = string.Empty;
    public string CorsOrigin { get; set; } = string.Empty;
    public string BaseStreamUrl { get; set; } = string.Empty;
    public string BaseImageUrl { get; set; } = string.Empty;
    public string RTMPUrl { get; set; } = string.Empty;
    public string RTMPSUrl { get; set; } = string.Empty;

    #endregion Public Properties
}

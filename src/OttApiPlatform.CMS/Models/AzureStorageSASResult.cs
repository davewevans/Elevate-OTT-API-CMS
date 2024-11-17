namespace OttApiPlatform.CMS.Models;

public class AzureStorageSASResult
{
    public string AccountName { get; set; }
    public Uri ContainerUri { get; set; }
    public string ContainerName { get; set; }
    public Uri SASUri { get; set; }
    public string SASToken { get; set; }
    public int SASExpireOnInMinutes { get; set; }
}

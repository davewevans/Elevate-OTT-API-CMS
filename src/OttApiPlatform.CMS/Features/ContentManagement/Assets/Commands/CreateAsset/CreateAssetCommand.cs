namespace OttApiPlatform.CMS.Features.ContentManagement.Assets.Commands.CreateAsset;

public class CreateAssetCommand
{
    public IBrowserFile File { get; set; }

    public AssetType Type { get; set; }

    public bool IsAlreadyUploaded { get; set; }

    public string FileName { get; set; }

    public string SasTokenUrl { get; set; }
}

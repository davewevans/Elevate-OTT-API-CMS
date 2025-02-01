using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetAssets;
public class AssetItem
{
    public Guid Id { get; set; }

    public AssetType Type { get; set; }

    public string Url { get; set; }

    public string DownloadUrl { get; set; }

    public string FileName { get; set; }

    public bool IsTemporary { get; set; } = true;

    public bool ClosedCaptions { get; set; }

    public bool IsTestAsset { get; set; }

    public AssetCreationStatus CreationStatus { get; set; }

    public Guid? LanguageId { get; set; }

    public Guid? ImageId { get; set; }

    public Guid? DocumentId { get; set; }

    public Guid? MuxAssetId { get; set; }
}

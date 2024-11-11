using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;
public class BlobOptions
{
    #region Public Fields

    public const string Section = "Blob";

    #endregion Public Fields

    #region Public Properties
    public string VideoBlobContainerName { get; set; } = string.Empty;
    public string ImageBlobContainerName { get; set; } = string.Empty;
    public string ContentFeedBlobContainerName { get; set; } = string.Empty;
    public int SASExpiresOnInMinutes { get; set; }
    public string BlobBaseUrl { get; set; } = string.Empty;

    #endregion Public Properties
}

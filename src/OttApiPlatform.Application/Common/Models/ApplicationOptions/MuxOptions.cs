using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

public class MuxOptions
{
    #region Public Fields

    public const string Section = "Mux";

    #endregion Public Fields

    #region Public Properties

    public string AccessTokenId { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;

    #endregion Public Properties
}

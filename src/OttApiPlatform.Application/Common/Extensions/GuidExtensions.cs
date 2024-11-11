using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Common.Extensions;

public static class GuidExtensions
{
    #region Public Methods

    /// <summary>
    /// Determines whether the specified GUID is null or empty.
    /// </summary>
    /// <param name="guid">The GUID to check.</param>
    /// <returns><c>true</c> if the specified GUID is null or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty(this Guid guid)
    {
        return guid == Guid.Empty;
    }

    /// <summary>
    /// Generates a storage name prefix from the specified GUID.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static string GetStorageFileNamePrefix(this Guid guid)
    {
        return guid.ToString().Replace("-", "");
    }

    #endregion Public Methods
}

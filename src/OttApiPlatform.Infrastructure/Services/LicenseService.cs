using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OttApiPlatform.Application.Common.Contracts;

namespace OttApiPlatform.Infrastructure.Services;
public class LicenseService : ILicenseService
{
    private readonly IConfigReaderService _configReaderService;

    public LicenseService(IConfigReaderService configReaderService)
    {
        _configReaderService = configReaderService;
    }

    public string GenerateLicenseForTenant(Guid tenantId)
    {
        var licenseInfoOptions = _configReaderService.GetLicenseInfoOptions();

        string productIdentifier = $"{licenseInfoOptions.ProductName}-{tenantId}-{licenseInfoOptions.Secret}";
        string md5Sum = GetMd5Sum(productIdentifier);
        return FormatLicenseKey(md5Sum);
    }

    #region Private Methods
    private string GetMd5Sum(string productIdentifier)
    {
        System.Text.Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
        byte[] unicodeText = new byte[productIdentifier.Length * 2];
        enc.GetBytes(productIdentifier.ToCharArray(), 0, productIdentifier.Length, unicodeText, 0, true);

        using (MD5 md5 = MD5.Create())
        {
            byte[] result = md5.ComputeHash(unicodeText);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    private string FormatLicenseKey(string productIdentifier)
    {
        productIdentifier = productIdentifier.Substring(0, 28).ToUpper();
        char[] serialArray = productIdentifier.ToCharArray();
        StringBuilder licenseKey = new StringBuilder();

        int j = 0;
        for (int i = 0; i < 28; i++)
        {
            for (j = i; j < 4 + i; j++)
            {
                licenseKey.Append(serialArray[j]);
            }
            if (j == 28)
            {
                break;
            }
            else
            {
                i = (j) - 1;
                licenseKey.Append("-");
            }
        }
        return licenseKey.ToString();
    }
    #endregion
}

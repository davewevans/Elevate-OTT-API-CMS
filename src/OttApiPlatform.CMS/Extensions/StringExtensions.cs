using System.Text.RegularExpressions;

namespace OttApiPlatform.CMS.Extensions;

public static class StringExtensions
{
    #region Public Methods

    public static string Filter(this string str, List<char> charsToRemove)
    {
        return charsToRemove.Aggregate(str, (current, c) => current.Replace(c.ToString(), string.Empty));
    }

    public static string ReplaceSpaceAndSpecialCharsWithDashes(this string str)
    {
        var cleanedStr = Regex.Replace(str, "[^a-zA-Z0-9_.-]+", "-", RegexOptions.Compiled).Replace(" ", "-");
        return cleanedStr;
    }

    public static string FormatSubdomain(this string str)
    {
        var cleanedStr = Regex.Replace(str, "[^a-zA-Z0-9_.-]+", "", RegexOptions.Compiled).Replace(" ", "").ToLower();
        return cleanedStr;
    }

    public static string FormatSlug(this string str)
    {
        return string.IsNullOrWhiteSpace(str) ? str : ReplaceSpaceAndSpecialCharsWithDashes(str).ToLower();
    }

    #endregion Public Methods
}
namespace OttApiPlatform.CMS.Extensions;

public static class StringExtensions
{
    #region Public Methods

    public static string Filter(this string str, List<char> charsToRemove)
    {
        return charsToRemove.Aggregate(str, (current, c) => current.Replace(c.ToString(), string.Empty));
    }

    #endregion Public Methods
}
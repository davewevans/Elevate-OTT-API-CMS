namespace OttApiPlatform.CMS.Extensions;

public static class NavigationManagerExtensions
{
    #region Public Methods

    public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T value)
    {
        var uri = navManager.ToAbsoluteUri(navManager.Uri);

        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
        {
            if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
            {
                value = (T)(object)valueAsInt;
                return true;
            }

            if (typeof(T) == typeof(string))
            {
                value = (T)(object)valueFromQueryString.ToString();
                return true;
            }

            if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
            {
                value = (T)(object)valueAsDecimal;
                return true;
            }
        }

        value = default;
        return false;
    }

    public static string GetSubDomain(this NavigationManager navManager)
    {
        var dot = '.';

        var navManagerBaseUri = navManager.BaseUri;

        var dotCount = navManagerBaseUri.Count(c => c == dot);

        switch (dotCount)
        {
            case 0 when navManagerBaseUri.Contains("localhost"):
            case 1 when !navManagerBaseUri.Contains("localhost"):
            case 2 when navManagerBaseUri.Contains("www"):
                return null;

            default:
                {
                    var subDomain = navManagerBaseUri.Split('.')[0].Split("//")[1];
                    return subDomain;
                }
        }
    }

    public static void NavigateToNewSubDomain(this NavigationManager navManager, string subdomain)
    {
        var baseUrl = navManager.Uri;
        var protocol = navManager.Uri.Split(':')[0];
        var baseUrlParts = baseUrl.Split('.');
        var domain = string.Join('.', baseUrlParts.Skip(1)).Split('/')[0];
        var newUrl = $"{protocol}://{subdomain}.{domain}/account/login";
        navManager.NavigateTo(newUrl, forceLoad: true, replace: true);
    }

    public static bool IsHost(this NavigationManager navManager)
    {
        var dot = '.';

        var navManagerBaseUri = navManager.BaseUri;

        var dotCount = navManagerBaseUri.Count(c => c == dot);

        return dotCount switch
        {
            0 when navManagerBaseUri.Contains("localhost") => true,
            1 when !navManagerBaseUri.Contains("localhost") => true,
            2 when navManagerBaseUri.Contains("www") => true,
            _ => false
        };
    }

    #endregion Public Methods
}
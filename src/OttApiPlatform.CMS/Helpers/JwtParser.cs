namespace OttApiPlatform.CMS.Helpers;

public static class JwtParser
{
    #region Public Methods

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs == null)
            return claims;
        keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

        if (roles != null)
        {
            if (roles.ToString().Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString() ?? string.Empty);
                claims.AddRange((parsedRoles ?? Array.Empty<string>()).Select(parsedRole => new Claim(ClaimTypes.Role, parsedRole)));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString() ?? string.Empty));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));

        return claims;
    }

    #endregion Public Methods

    #region Private Methods

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;

            case 3:
                base64 += "=";
                break;
        }
        return Convert.FromBase64String(base64);
    }

    #endregion Private Methods
}
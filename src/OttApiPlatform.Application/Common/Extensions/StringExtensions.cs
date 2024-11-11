namespace OttApiPlatform.Application.Common.Extensions;

public static class StringExtensions
{
    #region Public Methods

    /// <summary>
    /// Filters characters from the string based on a list of characters to remove.
    /// </summary>
    /// <param name="str">The input string to filter.</param>
    /// <param name="charsToRemove">The list of characters to remove from the string.</param>
    /// <returns>The filtered string.</returns>
    public static string Filter(this string str, List<char> charsToRemove)
    {
        return charsToRemove.Aggregate(str, (current, c) => current.Replace(c.ToString(), string.Empty));
    }

    /// <summary>
    /// Converts the input string to a URL-friendly string by replacing spaces, special characters,
    /// periods, and underscores with dashes and removing file extensions.
    /// </summary>
    /// <param name="input">The input string to convert.</param>
    /// <returns>A URL-friendly version of the input string.</returns>
    public static string ToUrlFriendlyString(this string input)
    {
        // Remove leading and trailing white spaces from the input string.
        input = input.Trim();

        // Remove file extension if it exists.
        if (Path.HasExtension(input))
            input = Path.GetFileNameWithoutExtension(input);

        // Remove leading and trailing white spaces from the input string.
        input = input.Trim();

        // Replace all "@" characters with dashes.
        input = input.Replace("@", "-");

        // Replace all non-alphanumeric characters with an empty string.
        input = Regex.Replace(input, @"[^a-zA-Z0-9_.]+", "-", RegexOptions.Compiled);

        // Replace all periods, underscores, and spaces with dashes.
        input = Regex.Replace(input, @"[._\s]+", "-", RegexOptions.Compiled);

        // Replace all sequences of dashes with a single dash.
        input = Regex.Replace(input, "-{2,}", "-", RegexOptions.Compiled);

        return input;
    }

    /// <summary>
    /// Removes all non-alphanumeric characters and spaces from the input string.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string RemoveNonAlphanumericCharsAndSpaces(this string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Value cannot be null or empty", nameof(input));

        // Remove non-alphanumeric characters and spaces from the input string.
        var cleanedInput = Regex.Replace(input, "[^a-zA-Z0-9]", "");
        
        return $"{cleanedInput.Replace(" ", "")}";
    }

    /// <summary>
    /// Replaces spaces and special characters with dashes in the input string.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ReplaceSpaceAndSpecialCharsWithDashes(this string str)
    {
        var cleanedStr = Regex.Replace(str.Replace("@", "-"), "[^a-zA-Z0-9_.]+", "-", RegexOptions.Compiled).Replace(" ", "-");
        return cleanedStr;
    }

    #endregion Public Methods
}
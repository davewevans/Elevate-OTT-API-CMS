namespace OttApiPlatform.Application.Common.Extensions;

public static class ValidationExtensions
{
    #region Public Methods

    /// <summary>
    /// Converts a collection of validation failures to a dictionary of property names and error messages.
    /// </summary>
    /// <param name="validationFailures">The collection of validation failures to convert.</param>
    /// <returns>A dictionary of property names and error messages.</returns>
    public static Dictionary<string, string> ToApplicationResult(this IEnumerable<ValidationFailure> validationFailures)
    {
        var rnd = new Random();
        return validationFailures.ToDictionary(validationFailure => validationFailure.PropertyName == string.Empty ? rnd.Next().ToString() : validationFailure.PropertyName, validationFailure => validationFailure.ErrorMessage);
    }

    /// <summary>
    /// Converts a collection of validation failures to a serialized string representation.
    /// </summary>
    /// <param name="validationFailures">The collection of validation failures to convert.</param>
    /// <returns>A serialized string representation of the validation failures.</returns>
    public static string ToSerializedResult(this IEnumerable<ValidationFailure> validationFailures)
    {
        var errorBuilder = new StringBuilder();

        foreach (var validationFailure in validationFailures)
        {
            errorBuilder.AppendLine(validationFailure.ErrorMessage);

            errorBuilder.AppendLine("||");
        }
        return errorBuilder.ToString();
    }

    /// <summary>
    /// Converts a collection of validation results to a serialized string representation.
    /// </summary>
    /// <param name="dbEntityValidationResults">The collection of validation results to convert.</param>
    /// <returns>A serialized string representation of the validation results.</returns>
    public static string ToSerializedResult(this IEnumerable<ValidationResult> dbEntityValidationResults)
    {
        var errorBuilder = new StringBuilder();

        foreach (var dbEntityValidationResult in dbEntityValidationResults)
            foreach (var error in dbEntityValidationResult.Errors)
            {
                errorBuilder.AppendLine($"{error.PropertyName}: {error.ErrorMessage}");

                errorBuilder.AppendLine("||");
            }

        return errorBuilder.ToString();
    }

    #endregion Public Methods
}
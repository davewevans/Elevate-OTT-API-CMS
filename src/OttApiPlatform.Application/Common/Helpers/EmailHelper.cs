namespace OttApiPlatform.Application.Common.Helpers
{
    public static class EmailHelper
    {
        public static string RemoveNonAlphanumericCharacters(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            // Split the email into local part and domain part
            var parts = email.Split('@');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid email format", nameof(email));

            var localPart = parts[0];

            // Remove non-alphanumeric characters from the local part
            var cleanedLocalPart = Regex.Replace(localPart, "[^a-zA-Z0-9]", "");

            // Return the cleaned email
            return $"{ cleanedLocalPart }";
        }
    }
}

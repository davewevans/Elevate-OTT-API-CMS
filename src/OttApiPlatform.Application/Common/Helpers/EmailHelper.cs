using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OttApiPlatform.Application.Common.Helpers
{
   public static class EmailHelper
    {
        public static string RemoveNonAlphanumericCharacters(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            // Split the email into local part and domain part
            var emailParts = email.Split('@');
            if (emailParts.Length != 2)
                throw new ArgumentException("Invalid email format", nameof(email));

            var localPart = emailParts[0];

            // Remove non-alphanumeric characters from the local part
            var cleanedLocalPart = Regex.Replace(localPart, "[^a-zA-Z0-9]", "");

            // Return the cleaned email
            return $"{ cleanedLocalPart }";
        }
    }
}

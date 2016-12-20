using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CharityKitchen
{
    /// <summary>
    /// A class with some small utilities used throught the project.
    /// </summary>
    public static class Utils
    {
        // http://stackoverflow.com/questions/1046740/how-can-i-validate-a-string-to-only-allow-alphanumeric-characters-in-it
        /// <summary>
        /// Checks a given string to see whether or not it is AlphaNumeric.
        /// </summary>
        /// <param name="str">The string to validate as AlphaNumeric.</param>
        /// <returns>true if the string is AlphaNumeric, false if it is not.</returns>
        public static bool IsAlphaNumeric(this string str)
        {
            if (str.All(char.IsLetterOrDigit))
                return true;
            else
                return false;
        }

        // http://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
        /// <summary>
        /// Checks a given string to see if it is in a valid format for an Email Address
        /// </summary>
        /// <param name="str">The string to validate as an Email Address format string.</param>
        /// <returns>true if it is in valid format for an Email Address, false if it is not.</returns>
        public static bool IsValidEmailAddress(this string str)
        {
            return new EmailAddressAttribute().IsValid(str);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Forestry
{
    /// <summary>
    /// Static class to implement method argument checks.
    /// </summary>
    public class Guard
    {
        public static void AgainstCondition(bool condition, string message = null)
        {
            if (condition)
                throw new Exception(message == null ? "An invalid condition encountered." : message);
        }

        public static void AgainstNull(object argValue, string argName, string message = null)
        {
            if (argValue == null)
                throw new ArgumentNullException(argName, message == null ? argName + " may not be null" : message);
        }

        public static void AgainstNonInt32(string shouldBeInt32)
        {
            int dummy;
            if (!Int32.TryParse(shouldBeInt32, out dummy))
                throw new ArgumentException(string.Format("'{0}' was exepcted to be an integer.", shouldBeInt32));
        }

        public static void AgainstNullOrEmptyString(string argumentValue, string argumentName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(argumentValue))
                throw new ArgumentException(message ?? "Parameter cannot be null or an empty string.", argumentName);
        }

        public static void AgainstNullOrEmptyCollection<T>(IEnumerable<T> argumentValue, string argumentName, string message = null)
        {
            if ((argumentValue == null) || (argumentValue.Count() == 0))
                throw new ArgumentException(message ?? "Parameter cannot be null or an empty collection.", argumentName);
        }

        public static void AgainstNegativeValue(int argumentValue, string argumentName, string message = null)
        {
            if (argumentValue < 0)
                throw new ArgumentException(message ?? "Parameter cannot be negative.", argumentName);
        }

        public static void AgainstNonPositiveValue(int argumentValue, string argumentName, string message = null)
        {
            if (argumentValue <= 0)
                throw new ArgumentException(message ?? "Parameter cannot be negative or zero.", argumentName);
        }

        public static void AgainstUnsupportedValues<T>(T argumentValue, string argumentName, IEnumerable<T> supportedValues, string message = null)
        {
            if ((supportedValues == null) || !supportedValues.Contains(argumentValue))
                throw new ArgumentException(
                    message
                    ?? string.Format("Argument value not supported. Supported values are {0}", string.Join(", ", supportedValues)),
                    argumentName);
        }

        public static void AgainstLongString(string argumentValue, string argumentName, int maxAcceptableLength, string message = null)
        {
            if (!string.IsNullOrWhiteSpace(argumentValue) && (argumentValue.Length > maxAcceptableLength))
                throw new ArgumentException(
                    message
                    ?? string.Format("String argument too long, {0} characters, max {1} allowed.", argumentValue.Length, maxAcceptableLength),
                    argumentName);
        }
    }
}

using System;
using System.Diagnostics.Contracts;
using System.Security;
using System.Text;

namespace LordJZ
{
    public static class StringExtensions
    {
        #region string.Format

        /// <summary>
        /// Replaces one or more format items in a specified string with the string representation of a specified object.
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="arg0">
        /// The object to format.
        /// </param>
        /// <returns>
        /// A copy of format in which any format items are replaced by the string representation of arg0.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <c>format</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// The format item in format is invalid.
        /// -or-
        /// The index of a format item is greater or less than zero.
        /// </exception>
        public static string Format(this string format, object arg0)
        {
            Contract.Requires(format != null, "format");

            return string.Format(format, arg0);
        }

        /// <summary>
        /// Replaces the format item in a specified string with the string representation
        /// of a corresponding object in a specified array.
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An object array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// format or args is null.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// format is invalid.-or- The index of a format item is less than zero, or greater
        /// than or equal to the length of the args array.
        /// </exception>
        public static string Format(string format, params object[] args)
        {
            Contract.Requires(format != null, "format");
            Contract.Requires(args != null, "args");

            return string.Format(format, args);
        }

        /// <summary>
        /// Replaces the format item in a specified string with the string representation
        /// of a corresponding object in a specified array. A specified parameter supplies
        /// culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        /// An object that supplies culture-specific formatting information.
        /// </param>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An object array that contains zero or more objects to format.
        /// </param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// format or args is null.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// format is invalid.
        /// -or-
        /// The index of a format item is less than zero, or greater
        /// than or equal to the length of the args array.
        /// </exception>
        [SecuritySafeCritical]
        public static string Format(IFormatProvider provider, string format, params object[] args)
        {
            Contract.Requires(format != null, "format");
            Contract.Requires(args != null, "args");

            return string.Format(provider, format, args);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representation
        /// of two specified objects.
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="arg0">
        /// The first object to format.
        /// </param>
        /// <param name="arg1">
        /// The second object to format.
        /// </param>
        /// <returns>
        /// A copy of format in which format items are replaced by the string representations
        /// of arg0 and arg1.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// format is null.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// format is invalid.
        /// -or-
        /// The index of a format item is less than zero, or greater than one.
        /// </exception>
        public static string Format(string format, object arg0, object arg1)
        {
            Contract.Requires(format != null, "format");

            return string.Format(format, arg0, arg1);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representation
        /// of three specified objects.
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="arg0">
        /// The first object to format.
        /// </param>
        /// <param name="arg1">
        /// The second object to format.
        /// </param>
        /// <param name="arg2">
        /// The third object to format.
        /// </param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string
        /// representations of arg0, arg1, and arg2.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// format is null.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// format is invalid.
        /// -or-
        /// The index of a format item is less than zero, or greater than two.
        /// </exception>
        public static string Format(string format, object arg0, object arg1, object arg2)
        {
            Contract.Requires(format != null, "format");

            return string.Format(format, arg0, arg1, arg2);
        }

        #endregion

        #region Urlize

        [Pure]
        static bool IsUrlCharacter(char c)
        {
            return c >= 'a' && c <= 'z' || c >= '0' && c <= '9' || c == '-';
        }

        [Pure]
        public static bool IsUrlized(this string text)
        {
            Contract.Requires(text != null);

            for (int i = 0, len = text.Length; i < len; i++)
            {
                if (!IsUrlCharacter(text[i]))
                    return false;
            }

            return true;
        }

        [Pure]
        public static string Urlize(this string text)
        {
            Contract.Requires(text != null);
            Contract.Ensures(Contract.Result<string>().IsUrlized());

            int length = text.Length;
            var builder = new StringBuilder(length);

            bool lastIsMinus = true;
            bool pendingMinus = false;
            for (int i = 0; i < length; i++)
            {
                var c = text[i];
                if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9')
                {
                    if (pendingMinus)
                    {
                        builder.Append('-');
                        pendingMinus = false;
                    }

                    builder.Append(char.ToLowerInvariant(c));
                    lastIsMinus = false;
                }
                else
                {
                    if (!lastIsMinus)
                        pendingMinus = true;
                }

            }

            return builder.ToString();
        }

        #endregion
    }
}

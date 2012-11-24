using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace LordJZ
{
    public static class AdvancedFormatExtensions
    {
        delegate string AdvancedFormatter(string format, ref int i, object obj, bool haveObj);

        static readonly SortedDictionary<string, AdvancedFormatter> s_advancedFormatters =
            new SortedDictionary<string, AdvancedFormatter>
            {
                // value ? first_arg : second_arg
                { "bool", AdvFormat_Bool },
                { "en-ordinal", AdvFormat_En_Ordinal },
                { "en-plural", AdvFormat_En_Plural },
                { "ru-plural", AdvFormat_Ru_Plural }
            };

        #region Language-agnostic

        static string AdvFormat_Bool(string format, ref int i, object obj, bool haveObj)
        {
            var ifTrue = ReadArgument(format, ref i, true);
            var ifFalse = ReadArgument(format, ref i, true);

            bool cond;
            string str;
            IConvertible convertible;

            if (!haveObj || obj == null)
            {
                cond = false;
            }
            else if ((str = obj as string) != null)
            {
                long value;

                if (string.IsNullOrEmpty(str))
                    cond = false;
                else if (bool.TryParse(str, out cond))
                {
                    // do nothing
                }
                else if (long.TryParse(str, out value))
                    cond = value != 0;
                else
                    // any other string is true
                    cond = true;
            }
            else if (obj is char)
            {
                cond = (char)obj != '\0';
            }
            else if ((convertible = obj as IConvertible) != null && !(obj is DateTime))
            {
                try
                {
                    cond = convertible.ToBoolean(null);
                }
                catch
                {
                    cond = false;
                }
            }
            else
                throw new ArgumentException("Cannot convert existing argument for advanced formatting 'bool' expression.");

            return cond ? ifTrue : ifFalse;
        }

        #endregion

        #region En

        static string AdvFormat_En_Ordinal(string format, ref int i, object obj, bool haveObj)
        {
            if (!haveObj)
                throw new ArgumentOutOfRangeException("format", "Argument index is out of range.");

            long signed;
            ulong unsigned;
            bool isSigned = ObjToInt(obj, out signed, out unsigned);

            // Negative and zero have no ordinal representation
            if (isSigned && signed <= 0 || !isSigned && unsigned == 0)
                return string.Empty;

            const string TH = "th";

            unsigned %= 100;
            if (unsigned >= 11 && unsigned <= 13)
                return TH;

            switch (unsigned % 10)
            {
                case 1: return "st";
                case 2: return "nd";
                case 3: return "rd";
                default: return TH;
            }
        }

        static string AdvFormat_En_Plural(string format, ref int i, object obj, bool haveObj)
        {
            if (!haveObj)
                throw new ArgumentOutOfRangeException("format", "Argument index is out of range.");

            var ifOne = ReadArgument(format, ref i, true);
            var ifMany = ReadArgument(format, ref i, true);

            long signed;
            ulong unsigned;
            bool isSigned = ObjToInt(obj, out signed, out unsigned);

            bool isOne;
            if (isSigned)
                isOne = signed == 1 || signed == -1;
            else
                isOne = unsigned == 1;

            return isOne ? ifOne : ifMany;
        }

        #endregion

        #region Ru

        static string AdvFormat_Ru_Plural(string format, ref int i, object obj, bool haveObj)
        {
            if (!haveObj)
                throw new ArgumentOutOfRangeException("format", "Argument index is out of range.");

            var one = ReadArgument(format, ref i, true);
            var two = ReadArgument(format, ref i, true);
            var three = ReadArgument(format, ref i, true);

            long signed;
            ulong unsigned;
            bool isSigned = ObjToInt(obj, out signed, out unsigned);
            if (isSigned)
                unsigned = (ulong)Math.Abs(signed);

            var tens = unsigned % 100;
            var ones = tens % 10;

            if (ones == 1 && tens != 11)
                return one;

            if (ones > 1 && ones < 5 && (tens < 10 || tens > 19))
                return two;

            return three;
        }

        #endregion

        /// <returns>is signed</returns>
        static bool ObjToInt(object obj, out long longVal, out ulong ulongVal)
        {
            if (obj is ulong)
            {
                ulongVal = (ulong)obj;

                if (ulongVal > long.MaxValue)
                {
                    longVal = 0;
                    return false;
                }

                longVal = (long)ulongVal;
                return true;
            }

            var convertible = obj as IConvertible;
            if (convertible != null)
            {
                try
                {
                    longVal = convertible.ToInt64(null);
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Failed to convert an argument to an integer.", e);
                }

                if (longVal < 0)
                {
                    ulongVal = 0;
                    return false;
                }

                ulongVal = (ulong)longVal;
                return true;
            }

            throw new ArgumentException("Failed to convert an argument to an integer.");
        }

        static string ReadArgument(string format, ref int i, bool advancedFormatting)
        {
            ++i;

            int length = format.Length;
            StringBuilder stringBuilder = null;

            while (true)
            {
                if (i == length)
                    throw new FormatException("Invalid format string.");

                Contract.Assert(i >= 0 && i < format.Length);

                char c = format[i];
                i++;
                if (c == '{')
                {
                    if (i < length && format[i] == '{')
                    {
                        i++;
                    }
                    else
                    {
                        throw new FormatException("Invalid format string.");
                    }
                }
                else if (c == '}')
                {
                    if (i >= length || format[i] != '}')
                        break;

                    i++;
                }
                else if (advancedFormatting && c == ':')
                    // no : escaping atm
                    break;

                if (stringBuilder == null)
                    stringBuilder = new StringBuilder();

                stringBuilder.Append(c);
            }

            i--;

            if (stringBuilder != null)
                return stringBuilder.ToString();

            if (advancedFormatting)
                throw new FormatException("Invalid number of arguments of advanced formatted object.");

            return null;
        }

        public static StringBuilder AppendAdvancedFormat(this StringBuilder builder,
            IFormatProvider provider, string format, params object[] args)
        {
            Contract.Requires(format != null, "format");
            Contract.Requires(args != null, "args");

            int i = 0;
            int length = format.Length;
            ICustomFormatter customFormatter = null;
            if (provider != null)
                customFormatter = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));

            while (true)
            {
                char c;
                if (i < length)
                {
                    c = format[i];
                    i++;
                    if (c == '}')
                    {
                        if (i < length && format[i] == '}')
                        {
                            i++;
                        }
                        else
                        {
                            throw new FormatException("Invalid format string.");
                        }
                    }
                    if (c == '{')
                    {
                        if (i >= length || format[i] != '{')
                        {
                            i--;
                            goto IL_9A;
                        }
                        i++;
                    }
                    builder.Append(c);
                    continue;
                }
            IL_9A:
                if (i == length)
                {
                    return builder;
                }
                i++;
                if (i == length || (c = format[i]) < '0' || c > '9')
                {
                    throw new FormatException("Invalid format string.");
                }
                int arg_index = 0;
                do
                {
                    arg_index = arg_index * 10 + c - 48;
                    i++;
                    if (i == length)
                    {
                        throw new FormatException("Invalid format string.");
                    }
                    c = format[i];
                }
                while (c >= '0' && c <= '9' && arg_index < 1000000);

                while (i < length && (c = format[i]) == ' ')
                    i++;

                bool rightAligned = false;
                int alignment = 0;
                if (c == ',')
                {
                    i++;
                    while (i < length && format[i] == ' ')
                    {
                        i++;
                    }
                    if (i == length)
                    {
                        throw new FormatException("Invalid format string.");
                    }
                    c = format[i];
                    if (c == '-')
                    {
                        rightAligned = true;
                        i++;
                        if (i == length)
                        {
                            throw new FormatException("Invalid format string.");
                        }
                        c = format[i];
                    }
                    if (c < '0' || c > '9')
                    {
                        throw new FormatException("Invalid format string.");
                    }
                    do
                    {
                        alignment = alignment * 10 + c - 48;
                        i++;
                        if (i == length)
                        {
                            throw new FormatException("Invalid format string.");
                        }
                        c = format[i];
                        if (c < '0' || c > '9')
                        {
                            break;
                        }
                    }
                    while (alignment < 1000000);
                }

                while (i < length && (c = format[i]) == ' ')
                    i++;

                //if (c == '?')
                //{
                //    // read advanced formatting
                //}
                //else
                //{

                string replacementFormat = null;
                string replacementText = null;

                bool advancedFormatting = c == '?';
                if (c == ':' || advancedFormatting)
                    replacementFormat = ReadArgument(format, ref i, advancedFormatting);

                if (advancedFormatting)
                {
                    AdvancedFormatter formatter;
                    if (!s_advancedFormatters.TryGetValue(replacementFormat, out formatter))
                        throw new FormatException("Invalid advanced format expression " + replacementFormat + ".");

                    bool haveObj = arg_index < args.Length;
                    replacementText = formatter(format, ref i, haveObj ? args[arg_index] : null, haveObj);
                }
                else
                {
                    if (arg_index >= args.Length)
                        break;

                    object obj = args[arg_index];

                    if (customFormatter != null)
                        replacementText = customFormatter.Format(replacementFormat, obj, provider);
                    else
                    {
                        var formattable = obj as IFormattable;
                        if (formattable != null)
                            replacementText = formattable.ToString(replacementFormat, provider);
                        else if (obj != null)
                            replacementText = obj.ToString();
                    }
                }

                Contract.Assert(i >= 0 && i < format.Length);

                c = format[i];
                if (c != '}')
                    throw new FormatException("Invalid format string for argument " + arg_index + ".");

                i++;

                if (replacementText == null)
                    replacementText = string.Empty;

                int spacesCount = alignment - replacementText.Length;
                if (!rightAligned && spacesCount > 0)
                {
                    builder.Append(' ', spacesCount);
                }
                builder.Append(replacementText);
                if (rightAligned && spacesCount > 0)
                {
                    builder.Append(' ', spacesCount);
                }
            }
            throw new FormatException("Argument index is out of range.");
        }

        public static string AdvancedFormat(IFormatProvider provider, string format, params object[] args)
        {
            Contract.Requires(format != null, "format");
            Contract.Requires(args != null, "args");

            var builder = new StringBuilder(format.Length + args.Length * 8);
            builder.AppendAdvancedFormat(provider, format, args);
            return builder.ToString();
        }

        public static string AdvancedFormat(this string format, IFormatProvider provider, params object[] args)
        {
            Contract.Requires(format != null, "format");
            Contract.Requires(args != null, "args");

            return AdvancedFormat(provider, format, args);
        }

        public static string AdvancedFormat(this string format, params object[] args)
        {
            Contract.Requires(format != null, "format");
            Contract.Requires(args != null, "args");

            return AdvancedFormat(null, format, args);
        }
    }
}

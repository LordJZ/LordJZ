using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using SystemEnum = System.Enum;

namespace LordJZ
{
    public struct EnumOrString<T> : IEquatable<EnumOrString<T>>/*,
        IEquatable<T>, IEquatable<string>*/
        where T : struct
    {
        #region Static Members

        static readonly MethodInfo s_GetCachedValuesAndNames;
        static readonly Type s_enumType;

        static EnumOrString()
        {
            s_enumType = typeof(T);

            Contract.Assert(s_enumType.IsEnum);

            Type runtimeTypeType = s_enumType.GetType();

            Contract.Assert(runtimeTypeType.Name == "RuntimeType");

            Type boolType = typeof(bool);

            s_GetCachedValuesAndNames = typeof(Enum).GetMethod(
                name: "GetCachedValuesAndNames",
                bindingAttr: BindingFlags.NonPublic | BindingFlags.Static,
                binder: null,
                types: new[]
                       {
                           runtimeTypeType,                     // 0 RuntimeType enumType
                           typeof(ulong[]).MakeByRefType(),     // 1 out ulong[] values
                           typeof(string[]).MakeByRefType(),    // 2 out string[] names
                           boolType,                            // 3 bool getValues
                           boolType                             // 4 bool getNames
                       },
                modifiers: null
                );

            Contract.Assert(s_GetCachedValuesAndNames != null);
        }

        static void GetValuesAndNames(out ulong[] values, out string[] names, bool getValues, bool getNames)
        {
            Contract.Ensures(!getValues || Contract.ValueAtReturn(out values) != null);
            Contract.Ensures(!getNames || Contract.ValueAtReturn(out names) != null);
            Contract.Ensures(
                !getValues || !getNames ||
                Contract.ValueAtReturn(out values).Length == Contract.ValueAtReturn(out names).Length
                );

            object[] parameters =
            {
                s_enumType,                     // 0 RuntimeType enumType
                null,                           // 1 out ulong[] values
                null,                           // 2 out string[] names
                BooleanBoxes.Box(getValues),    // 3 bool getValues
                BooleanBoxes.Box(getNames)      // 4 bool getNames
            };

            s_GetCachedValuesAndNames.Invoke(null, parameters);

            values = (ulong[])parameters[1];
            names = (string[])parameters[2];
        }

        #endregion

        readonly T m_enum;
        readonly string m_string;

        #region Constructors

        public EnumOrString(T value)
        {
            Contract.Requires(SystemEnum.IsDefined(typeof(T), value));

            m_enum = value;
            m_string = null;
        }

        public EnumOrString(string stringValue)
        {
            Contract.Requires(!string.IsNullOrEmpty(stringValue));

            ulong[] values;
            string[] names;
            GetValuesAndNames(out values, out names, true, true);

            object enumValue = null;

            int count = values.Length;
            Contract.Assert(count == names.Length);
            for (int i = 0; i < count; i++)
            {
                string name = names[i];
                if (name != stringValue)
                    continue;

                ulong value = values[i];
                enumValue = SystemEnum.ToObject(s_enumType, value);
                break;
            }

            if (enumValue != null)
            {
                m_enum = (T)enumValue;
                m_string = null;
            }
            else
            {
                m_enum = default(T);
                m_string = stringValue;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value indicating whether the current
        /// instance of <see cref="EnumOrString{T}"/> is an enum.
        /// </summary>
        public bool IsEnum
        {
            get { return m_string == null; }
        }

        /// <summary>
        /// Gets the value indicating whether the current
        /// instance of <see cref="EnumOrString{T}"/> is a string.
        /// </summary>
        public bool IsString
        {
            get { return m_string != null; }
        }

        /// <summary>
        /// Gets the string value of the current instance.
        /// Returned value is the string if the current instance is a string,
        /// or the enum value that is converted to string.
        /// </summary>
        public string String
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

                return m_string ?? m_enum.ToString();
            }
        }

        /// <summary>
        /// Gets the string value of the current instance.
        /// Returned value is the string if the current instance is a string,
        /// or the enum value that is converted to string.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This instance is not an enum.
        /// </exception>
        public T Enum
        {
            get
            {
                if (!this.IsEnum)
                    throw new InvalidOperationException("The current instance is a string.");

                return m_enum;
            }
        }

        #endregion

        #region Equality helpers

        public override bool Equals(object obj)
        {
            string str = obj as string;
            if (str != null)
                return this.Equals(str);

            if (obj is EnumOrString<T>)
                return this.Equals((EnumOrString<T>)obj);

            if (obj is T)
                return this.Equals((T)obj);

            return base.Equals(obj);
        }

        public bool Equals(EnumOrString<T> other)
        {
            if (this.IsEnum != other.IsEnum)
                return false;

            if (m_string != null)
                return m_string == other.m_string;

            return EqualityComparer<T>.Default.Equals(m_enum, other.m_enum);
        }

        public bool Equals(T other)
        {
            return this.Equals((EnumOrString<T>)other);
        }

        public bool Equals(string other)
        {
            return this.Equals((EnumOrString<T>)other);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return m_string != null ? m_string.GetHashCode() : m_enum.GetHashCode();
        }

        #endregion

        #region Conversion Operators

        public static implicit operator EnumOrString<T>(string operand)
        {
            return new EnumOrString<T>(operand);
        }

        public static implicit operator EnumOrString<T>(T operand)
        {
            return new EnumOrString<T>(operand);
        }

        public static implicit operator string(EnumOrString<T> operand)
        {
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            return operand.String;
        }

        public static implicit operator T(EnumOrString<T> operand)
        {
            return operand.Enum;
        }

        #endregion

        /// <summary>
        /// Gets the string value of the current instance.
        /// </summary>
        /// <returns>
        /// The string if the current instance is a string,
        /// or the enum value that is converted to string.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return this.String;
        }
    }
}

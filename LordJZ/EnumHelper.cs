using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace LordJZ
{
    public static class EnumHelper<T> where T : struct
    {
        #region ContractInvariant

        [ContractInvariantMethod]
        static void ContractInvariant()
        {
            Contract.Invariant(typeof(T).IsEnum);
        }

        #endregion

        #region Values

        static readonly LazyWeak<T[]> s_valuesLazy = new LazyWeak<T[]>(GetValues);

        public static T[] Values
        {
            get { return s_valuesLazy.Target; }
        }

        static T[] GetValues()
        {
            EnumMemberInfo<T>[] members;
            if (s_membersLazy.TryGetTarget(out members))
                return members.Select(member => member.Value).ToArray();

            return Enum.GetValues(typeof(T)).Cast<T>().OrderBy(t => t).ToArray();
        }

        #endregion

        static readonly LazyWeak<EnumMemberInfo<T>[]> s_membersLazy =
            new LazyWeak<EnumMemberInfo<T>[]>(GetMembers);

        public static EnumMemberInfo<T>[] Members
        {
            get { return s_membersLazy.Target; }
        }

        static EnumMemberInfo<T>[] GetMembers()
        {
            return Enum.GetValues(typeof(T))
                .Cast<object>()
                .Select(boxed => new EnumMemberInfo<T>(boxed.ToString(), boxed, (T)boxed))
                .ToArray();
        }
    }
}

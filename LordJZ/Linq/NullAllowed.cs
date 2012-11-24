
using System.Diagnostics.Contracts;

namespace LordJZ.Linq
{
    public struct NullAllowed<T> where T : class
    {
        public NullAllowed(T target)
        {
            this.m_target = target;
        }

        readonly T m_target;

        public T Target { get { return this.m_target; } }

        /// <summary>
        /// If the target is null, throwns an exception; otherwise, returns target.
        /// </summary>
        public T NonNullTarget
        {
            get
            {
                Contract.Ensures(Contract.Result<T>() != null);

                return m_target;
            }
        }

        public bool IsNull
        {
            get { return m_target == null; }
        }

        public bool IsNotNull
        {
            get { return m_target != null; }
        }
    }
}

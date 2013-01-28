using System;
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

                Contract.EndContractBlock();

                if (m_target == null)
                    throw new InvalidOperationException("Target is null.");

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

        [Pure]
        public TResult Cast<TResult>() where TResult : class
        {
            return m_target as TResult;
        }

        [Pure]
        public NullAllowed<TResult> CastNullable<TResult>() where TResult : class
        {
            return new NullAllowed<TResult>(m_target as TResult);
        }
    }
}

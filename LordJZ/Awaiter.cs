using System;
using System.Diagnostics.Contracts;

namespace LordJZ
{
    public struct Awaiter : IAwaiter
    {
        readonly IAwaitable m_awaitable;

        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(m_awaitable != null);
        }

        public Awaiter(IAwaitable awaitable)
        {
            Contract.Requires(awaitable != null);

            m_awaitable = awaitable;
        }

        public bool IsCompleted { get { return m_awaitable.IsCompleted; } }

        public void GetResult()
        {
            Contract.Requires(this.IsCompleted);

            m_awaitable.GetResult();
        }

        public void OnCompleted(Action action)
        {
            Contract.Requires(action != null);

            if (this.IsCompleted)
                action();
            else
                m_awaitable.OnCompleted(action);
        }
    }

    public struct Awaiter<T> : IAwaiter<T>
    {
        readonly IAwaitable<T> m_awaitable;

        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(m_awaitable != null);
        }

        public Awaiter(IAwaitable<T> awaitable)
        {
            Contract.Requires(awaitable != null);

            m_awaitable = awaitable;
        }

        public bool IsCompleted { get { return m_awaitable.IsCompleted; } }

        public T GetResult()
        {
            if (!this.IsCompleted)
                throw new InvalidOperationException();

            return m_awaitable.GetResult();
        }

        void IAwaiter.GetResult()
        {
            if (!this.IsCompleted)
                throw new InvalidOperationException();

            ((IAwaitable)m_awaitable).GetResult();
        }

        public void OnCompleted(Action action)
        {
            Contract.Requires(action != null);

            if (this.IsCompleted)
                action();
            else
                m_awaitable.OnCompleted(action);
        }
    }
}

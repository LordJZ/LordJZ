using System;
using System.Diagnostics.Contracts;

namespace LordJZ.Threading
{
    public struct Awaiter : IAwaiter
    {
        readonly IAwaitable m_awaitable;

        public Awaiter(IAwaitable awaitable)
        {
            Contract.Requires(awaitable != null);

            this.m_awaitable = awaitable;
        }

        public bool IsCompleted { get { return this.m_awaitable.IsCompleted; } }

        public void GetResult()
        {
            this.m_awaitable.GetResult();
        }

        public void OnCompleted(Action action)
        {
            Contract.Assume(action != null);

            if (this.IsCompleted)
                action();
            else
                this.m_awaitable.OnCompleted(action);
        }
    }

    public struct Awaiter<T> : IAwaiter<T>
    {
        readonly IAwaitable<T> m_awaitable;

        public Awaiter(IAwaitable<T> awaitable)
        {
            Contract.Requires(awaitable != null);

            this.m_awaitable = awaitable;
        }

        public bool IsCompleted { get { return this.m_awaitable.IsCompleted; } }

        public T GetResult()
        {
            return this.m_awaitable.GetResult();
        }

        void IAwaiter.GetResult()
        {
            ((IAwaitable)this.m_awaitable).GetResult();
        }

        public void OnCompleted(Action action)
        {
            Contract.Assume(action != null);

            if (this.IsCompleted)
                action();
            else
                this.m_awaitable.OnCompleted(action);
        }
    }
}

using System;
using System.Diagnostics.Contracts;

namespace LordJZ.Threading
{
    public sealed class WaitPoint : IAwaitable
    {
        Action m_onComplete;

        public bool IsCompleted { get; private set; }

        public void GetResult()
        {
        }

        IAwaiter IAwaitable.GetAwaiter()
        {
            return this.GetAwaiter();
        }

        public void Complete()
        {
            Contract.Requires(!this.IsCompleted);

            lock (this)
            {
                Contract.Assert(!this.IsCompleted);

                this.IsCompleted = true;

                Action onComplete = this.m_onComplete;
                this.m_onComplete = onComplete;

                if (onComplete != null)
                    onComplete();
            }
        }

        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }

        public void Reset()
        {
            lock (this)
                this.IsCompleted = false;
        }

        public void OnCompleted(Action action)
        {
            bool isComplete;

            lock (this)
            {
                isComplete = this.IsCompleted;
                if (!isComplete)
                    this.m_onComplete += action;
            }

            if (isComplete)
                action();
        }
    }

    public sealed class WaitPoint<T> : IAwaitable<T>
    {
        Action m_onComplete;
        T m_value;

        public bool IsCompleted { get; private set; }

        public T GetResult()
        {
            return this.m_value;
        }

        void IAwaiter.GetResult()
        {
        }

        IAwaiter IAwaitable.GetAwaiter()
        {
            return this.GetAwaiter();
        }

        public void Complete(T value)
        {
            Contract.Requires(!this.IsCompleted);

            lock (this)
            {
                Contract.Assert(!this.IsCompleted);

                this.m_value = value;
                this.IsCompleted = true;

                Action onComplete = this.m_onComplete;
                this.m_onComplete = onComplete;

                if (onComplete != null)
                    onComplete();
            }
        }

        public Awaiter<T> GetAwaiter()
        {
            return new Awaiter<T>(this);
        }

        public void Reset()
        {
            lock (this)
            {
                this.IsCompleted = false;
                this.m_value = default(T);
            }
        }

        IAwaiter<T> IAwaitable<T>.GetAwaiter()
        {
            return this.GetAwaiter();
        }

        public void OnCompleted(Action action)
        {
            Contract.Assume(action != null);

            bool isComplete;

            lock (this)
            {
                isComplete = this.IsCompleted;
                if (!isComplete)
                    this.m_onComplete += action;
            }

            if (isComplete)
                action();
        }
    }
}

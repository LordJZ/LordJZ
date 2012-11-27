using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace LordJZ.Threading.Contracts
{
    [ContractClassFor(typeof(IAwaiter))]
    internal abstract class IAwaiterContract : IAwaiter
    {
        public abstract bool IsCompleted { get; }
        public abstract void OnCompleted(Action continuation);

        void IAwaiter.GetResult()
        {
            Contract.Requires(this.IsCompleted);
        }
    }

    [ContractClassFor(typeof(IAwaiter<>))]
    internal abstract class IAwaiterContract<T> : IAwaiter<T>
    {
        public abstract bool IsCompleted { get; }
        public abstract void GetResult();
        public abstract void OnCompleted(Action continuation);

        T IAwaiter<T>.GetResult()
        {
            Contract.Requires(this.IsCompleted);

            return default(T);
        }
    }
}

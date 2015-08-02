using System;
using System.Diagnostics.Contracts;

namespace LordJZ.Threading.Contracts
{
#if CONTRACTS_FULL
    [ContractClassFor(typeof(IAwaitable))]
    internal abstract class IAwaitableContract : IAwaitable
    {
        public abstract void OnCompleted(Action continuation);
        public abstract bool IsCompleted { get; }
        public abstract void GetResult();

        IAwaiter IAwaitable.GetAwaiter()
        {
            Contract.Ensures(Contract.Result<IAwaiter>() != null);

            return null;
        }
    }

    [ContractClassFor(typeof(IAwaitable<>))]
    internal abstract class IAwaitableContract<T> : IAwaitable<T>
    {
        public abstract void OnCompleted(Action continuation);
        public abstract bool IsCompleted { get; }
        public abstract T GetResult();
        public abstract IAwaiter GetAwaiter();

        void IAwaiter.GetResult()
        {
        }

        IAwaiter<T> IAwaitable<T>.GetAwaiter()
        {
            Contract.Ensures(Contract.Result<IAwaiter>() != null);

            return null;
        }
    }
#endif
}

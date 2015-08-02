using System.Diagnostics.Contracts;
using LordJZ.Threading.Contracts;

namespace LordJZ.Threading
{
#if CONTRACTS_FULL
    [ContractClass(typeof(IAwaitableContract))]
#endif
    public interface IAwaitable : IAwaiter
    {
        IAwaiter GetAwaiter();
    }

#if CONTRACTS_FULL
    [ContractClass(typeof(IAwaitableContract<>))]
#endif
    public interface IAwaitable<out T> : IAwaitable, IAwaiter<T>
    {
        new IAwaiter<T> GetAwaiter();
    }
}

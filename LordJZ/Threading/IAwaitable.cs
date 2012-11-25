using System.Diagnostics.Contracts;
using LordJZ.Threading.Contracts;

namespace LordJZ.Threading
{
    [ContractClass(typeof(IAwaitableContract))]
    public interface IAwaitable : IAwaiter
    {
        IAwaiter GetAwaiter();
    }

    [ContractClass(typeof(IAwaitableContract<>))]
    public interface IAwaitable<out T> : IAwaitable, IAwaiter<T>
    {
        new IAwaiter<T> GetAwaiter();
    }
}

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using LordJZ.Threading.Contracts;

namespace LordJZ.Threading
{
    [ContractClass(typeof(IAwaiterContract))]
    public interface IAwaiter : INotifyCompletion
    {
        bool IsCompleted { get; }

        void GetResult();
    }

    [ContractClass(typeof(IAwaiterContract<>))]
    public interface IAwaiter<out T> : IAwaiter
    {
        new T GetResult();
    }
}

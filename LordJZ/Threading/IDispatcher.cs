using System;

namespace LordJZ.Threading
{
    public interface IDispatcher
    {
        bool InvokeRequired { get; }

        void Invoke(Action action);
        T Invoke<T>(Func<T> func);
        void BeginInvoke(Action action);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ
{
    public interface IAwaitable : IAwaiter
    {
        IAwaiter GetAwaiter();
    }

    public interface IAwaitable<out T> : IAwaitable, IAwaiter<T>
    {
        new IAwaiter<T> GetAwaiter();
    }
}

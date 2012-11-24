using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LordJZ.Collections.Contracts;

namespace LordJZ.Collections
{
    [ContractClass(typeof(ILongCollectionContract<>))]
    public interface ILongCollection<out T> : IEnumerable<T>
    {
        long LongCount { get; }
    }
}

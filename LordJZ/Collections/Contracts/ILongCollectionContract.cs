using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace LordJZ.Collections.Contracts
{
    [ContractClassFor(typeof(ILongCollection<>))]
    internal abstract class ILongCollectionContract<T> : ILongCollection<T>
    {
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long LongCount
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() >= 0);

                return default(long);
            }
        }
    }
}

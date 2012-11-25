using System;
using System.Diagnostics.Contracts;

namespace LordJZ.Contracts
{
    [ContractClassFor(typeof(IEnumMemberInfo))]
    internal abstract class IEnumMemberInfoContract : IEnumMemberInfo
    {
        string IEnumMemberInfo.Name
        {
            get
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

                return null;
            }
        }

        object IEnumMemberInfo.Value
        {
            get
            {
                Contract.Ensures(Contract.Result<object>() != null);

                return null;
            }
        }
    }
}

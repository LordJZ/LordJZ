using System.Diagnostics.Contracts;
using LordJZ.Contracts;

namespace LordJZ
{
    [ContractClass(typeof(IEnumMemberInfoContract))]
    public interface IEnumMemberInfo
    {
        string Name { get; }
        object Value { get; }
    }
}

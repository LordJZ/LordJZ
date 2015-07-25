using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.WinAPI
{
    [Flags]
    public enum FileMappingSecurity
    {
        Copy = 1,
        Write = 2,
        Read = 4,
        Execute = 0x20,

        /// <remarks>
        /// <see cref="Execute"/> is not included.
        /// </remarks>
        AllAccess = 0xF001F,

        /// <remarks>
        /// Available starting with Windows 10.
        /// </remarks>
        TargetsInvalid = 0x40000000,

        /// <remarks>
        /// 0x80000000
        /// </remarks>
        Reserve = -2147483648
    }
}

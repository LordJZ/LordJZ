using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.WinAPI
{
    [Flags]
    public enum MemoryState
    {
        /// <summary>
        /// Indicates committed pages for which physical storage has been allocated, either in memory or in the paging file on disk.
        /// </summary>
        Commit = 0x1000,

        /// <summary>
        /// Indicates free pages not accessible to the calling process and available to be allocated.
        /// </summary>
        Free = 0x10000,

        /// <summary>
        /// Indicates reserved pages where a range of the process's virtual address space is reserved without any physical storage being allocated.
        /// </summary>
        Reserve = 0x2000,
    }
}

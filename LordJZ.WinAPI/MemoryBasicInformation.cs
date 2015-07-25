using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public sealed class MemoryBasicInformation
    {
        public readonly IntPtr BaseAddress;
        public readonly IntPtr AllocationBase;
        public readonly MemoryProtection AllocationProtect;
        public readonly long RegionSize;
        public readonly MemoryState State;
        public readonly MemoryProtection Protect;
        public readonly MemoryType Type;

        public MemoryBasicInformation()
        {
        }

        internal MemoryBasicInformation(ref MEMORY_BASIC_INFORMATION64 info)
        {
            this.BaseAddress = (IntPtr)info.BaseAddress;
            this.AllocationBase = (IntPtr)info.AllocationBase;
            this.AllocationProtect = (MemoryProtection)info.AllocationProtect;
            this.RegionSize = info.RegionSize;
            this.State = (MemoryState)info.State;
            this.Protect = (MemoryProtection)info.Protect;
            this.Type = (MemoryType)info.Type;
        }

        internal MemoryBasicInformation(ref MEMORY_BASIC_INFORMATION32 info)
        {
            this.BaseAddress = (IntPtr)info.BaseAddress;
            this.AllocationBase = (IntPtr)info.AllocationBase;
            this.AllocationProtect = (MemoryProtection)info.AllocationProtect;
            this.RegionSize = info.RegionSize;
            this.State = (MemoryState)info.State;
            this.Protect = (MemoryProtection)info.Protect;
            this.Type = (MemoryType)info.Type;
        }
    }
}

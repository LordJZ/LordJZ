using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.WinAPI.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct IMAGE_DATA_DIRECTORY
    {
        public Int32 VirtualAddress;
        public Int32 Size;
    }
}

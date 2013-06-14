using System;
using System.Runtime.InteropServices;

namespace LordJZ.WinAPI.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct IMAGE_EXPORT_DIRECTORY
    {
        public UInt32 Characteristics;
        public UInt32 TimeDateStamp;
        public UInt16 MajorVersion;
        public UInt16 MinorVersion;
        public UInt32 Name;
        public UInt32 Base;
        public Int32 NumberOfFunctions;
        public Int32 NumberOfNames;
        public Int32 AddressOfFunctions;     // RVA from base of image
        public Int32 AddressOfNames;     // RVA from base of image
        public Int32 AddressOfNameOrdinals;  // RVA from base of image
    }
}

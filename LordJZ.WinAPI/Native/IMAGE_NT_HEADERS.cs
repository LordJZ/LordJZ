using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.WinAPI.Native
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct IMAGE_NT_HEADERS
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] Signature;

        [FieldOffset(4)]
        public IMAGE_FILE_HEADER FileHeader;

        [FieldOffset(24)]
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader32;

        [FieldOffset(24)]
        public IMAGE_OPTIONAL_HEADER64 OptionalHeader64;

        public bool Is64
        {
            get
            {
                switch (OptionalHeader32.Magic)
                {
                    case MagicType.IMAGE_NT_OPTIONAL_HDR32_MAGIC:
                        return false;
                    case MagicType.IMAGE_NT_OPTIONAL_HDR64_MAGIC:
                        return true;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return Signature[0] == 'P' && Signature[1] == 'E' && Signature[2] == 0 && Signature[3] == 0 &&
                       FileHeader.SizeOfOptionalHeader >= 200 &&
                       (OptionalHeader32.Magic == MagicType.IMAGE_NT_OPTIONAL_HDR32_MAGIC ||
                        OptionalHeader32.Magic == MagicType.IMAGE_NT_OPTIONAL_HDR64_MAGIC);
            }
        }
    }
}

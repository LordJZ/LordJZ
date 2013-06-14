using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.WinAPI.Native
{
    internal enum ImageDirectoryEntry
    {
        IMAGE_DIRECTORY_ENTRY_EXPORT          = 0,   // Export Directory
        IMAGE_DIRECTORY_ENTRY_IMPORT          = 1,   // Import Directory
        IMAGE_DIRECTORY_ENTRY_RESOURCE        = 2,   // Resource Directory
        IMAGE_DIRECTORY_ENTRY_EXCEPTION       = 3,   // Exception Directory
        IMAGE_DIRECTORY_ENTRY_SECURITY        = 4,   // Security Directory
        IMAGE_DIRECTORY_ENTRY_BASERELOC       = 5,   // Base Relocation Table
        IMAGE_DIRECTORY_ENTRY_DEBUG           = 6,   // Debug Directory
        IMAGE_DIRECTORY_ENTRY_ARCHITECTURE    = 7,   // Architecture Specific Data
        IMAGE_DIRECTORY_ENTRY_GLOBALPTR       = 8,   // RVA of GP
        IMAGE_DIRECTORY_ENTRY_TLS             = 9,   // TLS Directory
        IMAGE_DIRECTORY_ENTRY_LOAD_CONFIG    = 10,   // Load Configuration Directory
        IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT   = 11,   // Bound Import Directory in headers
        IMAGE_DIRECTORY_ENTRY_IAT            = 12,   // Import Address Table
        IMAGE_DIRECTORY_ENTRY_DELAY_IMPORT   = 13,   // Delay Load Import Descriptors
        IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR = 14,   // COM Runtime descriptor
    }
}

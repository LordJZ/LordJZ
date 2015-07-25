using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordJZ.WinAPI
{
    [Flags]
    public enum MemoryProtection
    {
        /// <summary>
        /// Enables execute access to the committed region of pages.
        /// </summary>
        /// <remarks>
        /// An attempt to write to the committed region results in an access violation.
        /// </remarks>
        Execute = 0x10,

        /// <summary>
        /// Enables execute or read-only access to the committed region of pages.
        /// </summary>
        /// <remarks>
        /// An attempt to write to the committed region results in an access violation.
        /// </remarks>
        ExecuteRead = 0x20,

        /// <summary>
        /// Enables execute, read-only, or read/write access to the committed region of pages.
        /// </summary>
        ExecuteReadWrite = 0x40,

        /// <summary>
        /// Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object.
        /// </summary>
        /// <remarks>
        /// An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process.
        /// The private page is marked as <see cref="ExecuteReadWrite"/>, and the change is written to the new page.
        /// </remarks>
        ExecuteWriteCopy = 0x80,

        /// <summary>
        /// Disables all access to the committed region of pages.
        /// </summary>
        /// <remarks>
        /// An attempt to read from, write to, or execute the committed region results in an access violation.
        /// </remarks>
        NoAccess = 0x01,

        /// <summary>
        /// Enables read-only access to the committed region of pages.
        /// </summary>
        /// <remarks>
        /// An attempt to write to the committed region results in an access violation.
        /// If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
        /// </remarks>
        ReadOnly = 0x02,

        /// <summary>
        /// Enables read-only or read/write access to the committed region of pages.
        /// </summary>
        /// <remarks>
        /// If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        /// </remarks>
        ReadWrite = 0x04,

        /// <summary>
        /// Enables read-only or copy-on-write access to a mapped view of a file mapping object.
        /// </summary>
        /// <remarks>
        /// An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process.
        /// The private page is marked as <see cref="ReadWrite"/>, and the change is written to the new page.
        /// If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        /// </remarks>
        WriteCopy = 0x08,

        /// <summary>
        /// Sets all locations in the pages as invalid targets for CFG.
        /// </summary>
        /// <remarks>
        /// Used along with any execute page protection like <see cref="Execute"/>, <see cref="ExecuteRead"/>,
        /// <see cref="ExecuteReadWrite"/> and <see cref="ExecuteWriteCopy"/>.
        /// Any indirect call to locations in those pages will fail CFG checks and the process will be terminated.
        /// The default behavior for executable pages allocated is to be marked valid call targets for CFG.
        /// </remarks>
        TargetsInvalid = 0x40000000,

        /// <summary>
        /// Pages in the region will not have their CFG information updated while the protection changes for VirtualProtect.
        /// </summary>
        /// <remarks>
        /// For example, if the pages in the region was allocated using <see cref="TargetsInvalid"/>,
        /// then the invalid information will be maintained while the page protection changes.
        /// This flag is only valid when the protection changes to an executable type
        /// like <see cref="Execute"/>, <see cref="ExecuteRead"/>, <see cref="ExecuteReadWrite"/> and <see cref="ExecuteWriteCopy"/>.
        /// The default behavior for VirtualProtect protection change to executable is to mark all locations as valid call targets for CFG.
        /// </remarks>
        TargetsNoUpdate = 0x40000000,

        /// <summary>
        /// Pages in the region become guard pages.
        /// </summary>
        /// <remarks>
        /// Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception
        /// and turn off the guard page status. Guard pages thus act as a one-time access alarm.
        ///
        /// When an access attempt leads the system to turn off guard page status, the underlying page protection takes over.
        ///
        /// If a guard page exception occurs during a system service, the service typically returns a failure status indicator.
        ///
        /// This value cannot be used with <see cref="NoAccess"/>.
        /// </remarks>
        Guard = 0x100,

        /// <summary>
        /// Sets all pages to be non-cachable.
        /// </summary>
        /// <remarks>
        /// Applications should not use this attribute except when explicitly required for a device.
        /// Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
        ///
        /// The <see cref="NoCache"/> flag cannot be used with the <see cref="Guard"/>, <see cref="NoAccess"/>, or <see cref="WriteCombine"/> flags.
        ///
        /// The <see cref="NoCache"/> flag can be used only when allocating private memory with the
        /// VirtualAlloc, VirtualAllocEx, or VirtualAllocExNuma functions.
        /// To enable non-cached memory access for shared memory, specify the SEC_NOCACHE flag when calling the CreateFileMapping function.
        /// </remarks>
        NoCache = 0x200,

        /// <summary>
        /// Sets all pages to be write-combined.
        /// </summary>
        /// <remarks>
        /// Applications should not use this attribute except when explicitly required for a device.
        /// Using the interlocked functions with memory that is mapped as write-combined can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
        ///
        /// The <see cref="WriteCombine"/> flag cannot be specified with the <see cref="NoAccess"/>, <see cref="Guard"/>, and <see cref="NoCache"/> flags.
        ///
        /// The <see cref="WriteCombine"/> flag can be used only when allocating private memory with the
        /// VirtualAlloc, VirtualAllocEx, or VirtualAllocExNuma functions.
        /// To enable write-combined memory access for shared memory, specify the SEC_WRITECOMBINE flag when calling the CreateFileMapping function.
        /// </remarks>
        WriteCombine = 0x400,
    }
}

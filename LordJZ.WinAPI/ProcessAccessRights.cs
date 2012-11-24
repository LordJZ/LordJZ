using System;

namespace LordJZ.WinAPI
{
    [Flags]
    public enum ProcessAccessRights
    {
        /// <summary>
        /// Required to create a process.
        /// </summary>
        CreateProcess = 0x0080,

        /// <summary>
        /// Required to create a thread.
        /// </summary>
        CreateThread = 0x0002,

        /// <summary>
        /// Required to duplicate a handle using <see cref="Handle.Duplicate"/>.
        /// </summary>
        DuplicateHandle = 0x0040,

        /// <summary>
        /// Required to retrieve certain information about a process, such as its token,
        /// exit code, and priority class (see <see cref="ProcessToken"/>).
        /// </summary>
        QueryInformation = 0x0400,

        /// <summary>
        /// Required to retrieve certain information about a process (see
        /// GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName).
        /// A handle that has the QueryInformation access right
        /// is automatically granted QueryLimitedInformation.
        /// </summary>
        /// <remarks>
        /// Windows Server 2003 and Windows XP:  This access right is not supported.
        /// </remarks>
        QueryLimitedInformation = 0x1000,

        /// <summary>
        /// Required to set certain information about a process,
        /// such as its priority class (see SetPriorityClass).
        /// </summary>
        SetInformation = 0x0200,

        /// <summary>
        /// Required to set memory limits using SetProcessWorkingSetSize.
        /// </summary>
        SetQuota = 0x0100,

        /// <summary>
        /// Required to suspend or resume a process.
        /// </summary>
        SuspendResume = 0x0800,

        /// <summary>
        /// Required to terminate a process using <see cref="Process.Terminate"/>.
        /// </summary>
        Terminate = 0x0001,

        /// <summary>
        /// Required to perform an operation on the address space of a process
        /// (see VirtualProtectEx and WriteProcessMemory).
        /// </summary>
        VirtualMemoryOperation = 0x0008,

        /// <summary>
        /// Required to read memory in a process using ReadProcessMemory.
        /// </summary>
        VirtualMemoryRead = 0x0010,

        /// <summary>
        /// Required to write to memory in a process using WriteProcessMemory.
        /// </summary>
        VirtualMemoryWrite = 0x0020,

        /// <summary>
        /// Required to wait for the process to terminate using the wait functions.
        /// </summary>
        Synchronize = 0x00100000,
    }
}

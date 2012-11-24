using System;

namespace LordJZ.WinAPI
{
    [Flags]
    public enum HandleFlags : uint
    {
        None = 0,

        /// <summary>
        /// If this flag is set, a child process created with the bInheritHandles
        /// parameter of CreateProcess set to TRUE will inherit the object handle.
        /// </summary>
        Inherit = 1,

        /// <summary>
        /// If this flag is set, calling the <see cref="Handle.Close"/> function
        /// will not close the object handle.
        /// </summary>
        ProtectFromClose = 2,

        All = Inherit | ProtectFromClose,
    }
}

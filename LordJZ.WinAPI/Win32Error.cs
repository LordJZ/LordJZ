
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LordJZ.WinAPI
{
    public static class Win32Error
    {
        public static int Last { get { return Marshal.GetLastWin32Error(); } }

        public static bool IsSuccess(int error)
        {
            return error >= 0;
        }

        public static void EnsureSuccess(int error)
        {
            if (!IsSuccess(error))
                throw new Win32Exception(error);
        }

        internal static void EnsureNoWin32Error()
        {
            int error = Last;
            if (!IsSuccess(error))
                throw new Win32Exception(error);
        }

        internal static void EnsureNoWin32Error(this bool isExpectedResult)
        {
            int error = Last;
            if (!isExpectedResult || !IsSuccess(error))
                throw new Win32Exception(error);
        }

        public const int PartialCopy = 299;
    }
}

using System;
using System.Diagnostics.Contracts;
using LordJZ.WinAPI.Native;

namespace LordJZ.WinAPI
{
    public struct Handle : IEquatable<Handle>, IDisposable
    {
        public static readonly Handle InvalidHandle = new Handle(new IntPtr(-1));
        public static readonly Handle NullHandle = new Handle(IntPtr.Zero);

        readonly IntPtr m_handle;

        public Handle(IntPtr handle)
        {
            m_handle = handle;
        }

        public bool Close()
        {
            return UnsafeNativeMethods.CloseHandle(m_handle);
        }

        public Handle Duplicate()
        {
            throw new NotImplementedException();
        }

        #region Flags

        /// <summary>
        /// If this flag is set, a child process created with the bInheritHandles
        /// parameter of CreateProcess set to TRUE will inherit the object handle.
        /// </summary>
        public bool Inherited
        {
            get { return (this.Flags & HandleFlags.Inherit) != 0; }
            set { SetFlags(HandleFlags.Inherit, value ? HandleFlags.Inherit : HandleFlags.None); }
        }

        /// <summary>
        /// If this flag is set, calling the <see cref="Handle.Close"/> function
        /// will not close the object handle.
        /// </summary>
        public bool ProtectedFromClose
        {
            get { return (this.Flags & HandleFlags.ProtectFromClose) != 0; }
            set
            {
                HandleFlags newValue = value ? HandleFlags.ProtectFromClose : HandleFlags.None;
                SetFlags(HandleFlags.ProtectFromClose, newValue);
            }
        }

        public HandleFlags Flags
        {
            get
            {
                uint flags;
                UnsafeNativeMethods.GetHandleInformation(m_handle, out flags).EnsureNoWin32Error();
                return (HandleFlags)flags;
            }
            set { SetFlags(HandleFlags.All, value); }
        }

        void SetFlags(HandleFlags mask, HandleFlags value)
        {
            UnsafeNativeMethods.SetHandleInformation(m_handle, (uint)mask, (uint)value).EnsureNoWin32Error();
        }

        #endregion

        public IntPtr Value
        {
            get { return m_handle; }
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        #region Equality helpers

        public bool Equals(Handle other)
        {
            return m_handle == other.m_handle;
        }

        public override bool Equals(object obj)
        {
            if (obj is Handle)
                return this.Equals((Handle)obj);

            return false;
        }

        public override int GetHashCode()
        {
            return m_handle.GetHashCode();
        }

        public override string ToString()
        {
            return m_handle.ToString("X");
        }

        public static bool operator==(Handle handle1, Handle handle2)
        {
            return handle1.m_handle == handle2.m_handle;
        }

        public static bool operator !=(Handle handle1, Handle handle2)
        {
            return handle1.m_handle != handle2.m_handle;
        }

        #endregion
    }
}

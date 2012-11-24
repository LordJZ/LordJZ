using System;
using System.Threading;

namespace LordJZ.Threading
{
    public struct RWLockSlimWriteGuard : IDisposable
    {
        ReaderWriterLockSlim m_lockSlim;

        internal RWLockSlimWriteGuard(ReaderWriterLockSlim lockSlim)
        {
            m_lockSlim = lockSlim;
        }

        public void Dispose()
        {
            if (m_lockSlim != null)
            {
                m_lockSlim.ExitWriteLock();
                m_lockSlim = null;
            }
        }
    }

    public struct RWLockSlimReadGuard : IDisposable
    {
        ReaderWriterLockSlim m_lockSlim;

        internal RWLockSlimReadGuard(ReaderWriterLockSlim lockSlim)
        {
            m_lockSlim = lockSlim;
        }

        public void Dispose()
        {
            if (m_lockSlim != null)
            {
                m_lockSlim.ExitReadLock();
                m_lockSlim = null;
            }
        }
    }

    public struct RWLockSlimUpgradeableReadGuard : IDisposable
    {
        ReaderWriterLockSlim m_lockSlim;

        internal RWLockSlimUpgradeableReadGuard(ReaderWriterLockSlim lockSlim)
        {
            m_lockSlim = lockSlim;
        }

        public void Dispose()
        {
            if (m_lockSlim != null)
            {
                m_lockSlim.ExitUpgradeableReadLock();
                m_lockSlim = null;
            }
        }
    }
}

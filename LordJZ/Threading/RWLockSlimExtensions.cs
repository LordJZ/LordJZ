using System.Threading;

namespace LordJZ.Threading
{
    public static class RWLockSlimExtensions
    {
        public static RWLockSlimWriteGuard LockWrite(this ReaderWriterLockSlim lockSlim)
        {
            if (lockSlim != null)
                lockSlim.EnterWriteLock();

            return new RWLockSlimWriteGuard(lockSlim);
        }

        public static RWLockSlimReadGuard LockRead(this ReaderWriterLockSlim lockSlim)
        {
            if (lockSlim != null)
                lockSlim.EnterReadLock();

            return new RWLockSlimReadGuard(lockSlim);
        }

        public static RWLockSlimUpgradeableReadGuard LockUpgradeableRead(this ReaderWriterLockSlim lockSlim)
        {
            if (lockSlim != null)
                lockSlim.EnterUpgradeableReadLock();

            return new RWLockSlimUpgradeableReadGuard(lockSlim);
        }
    }
}

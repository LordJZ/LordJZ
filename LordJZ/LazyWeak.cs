using System;
using System.Diagnostics.Contracts;

namespace LordJZ
{
    public class LazyWeak<T> : WeakReference where T : class
    {
        readonly Func<T> m_factory;

        public LazyWeak(Func<T> factory)
            : base(null)
        {
            Contract.Requires(factory != null);

            m_factory = factory;
        }

        public LazyWeak(Func<T> factory, T target)
            : base(target)
        {
            Contract.Requires(factory != null);

            m_factory = factory;
        }

        public new T Target
        {
            get
            {
                var target = (T)base.Target;
                if (target == null)
                {
                    target = m_factory();
                    Contract.Assert(target != null);
                    base.Target = target;
                }

                return target;
            }
            set
            {
                base.Target = value;
            }
        }

        public bool TryGetTarget(out T target)
        {
            target = (T)base.Target;
            return target != null;
        }
    }
}

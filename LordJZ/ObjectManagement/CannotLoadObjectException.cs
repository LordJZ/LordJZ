using System;

namespace LordJZ.ObjectManagement
{
    [Serializable]
    public sealed class CannotLoadObjectException : Exception
    {
        public CannotLoadObjectException()
        {
        }

        public CannotLoadObjectException(string message)
            : base(message)
        {
        }

        public CannotLoadObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

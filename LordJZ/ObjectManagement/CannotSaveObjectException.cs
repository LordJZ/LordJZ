using System;

namespace LordJZ.ObjectManagement
{
    [Serializable]
    public sealed class CannotSaveObjectException : Exception
    {
        public CannotSaveObjectException()
        {
        }

        public CannotSaveObjectException(string message)
            : base(message)
        {
        }

        public CannotSaveObjectException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

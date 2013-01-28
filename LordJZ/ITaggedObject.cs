
using System.Diagnostics.Contracts;

namespace LordJZ
{
    /// <summary>
    /// Specifies an object that carries a <see cref="Tag"/> object.
    /// This tag should not be used by the object itself, but
    /// rather by the users of the implementing object.
    /// </summary>
    public interface ITaggedObject
    {
        /// <summary>
        /// An arbitrary object that is associated
        /// with the current instance by its users.
        /// This tag should not be used by the object itself.
        /// </summary>
        [Pure]
        object Tag { get; set; }
    }
}

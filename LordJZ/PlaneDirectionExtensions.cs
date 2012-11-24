using System;

namespace LordJZ
{
    public static class PlaneDirectionExtensions
    {
        /// <summary>
        /// Gets the <see cref="LordJZ.PlaneDirection"/> that is opposite
        /// to the provided <see cref="LordJZ.PlaneDirection"/>.
        /// </summary>
        /// <param name="direction">
        /// The <see cref="LordJZ.PlaneDirection"/> which
        /// opposite <see cref="LordJZ.PlaneDirection"/> is requested.
        /// </param>
        /// <returns>
        /// The <see cref="LordJZ.PlaneDirection"/> that is opposite
        /// to the provided <see cref="LordJZ.PlaneDirection"/>.
        /// </returns>
        public static PlaneDirection Opposite(this PlaneDirection direction)
        {
            switch (direction)
            {
                case PlaneDirection.Left:
                    return PlaneDirection.Right;
                case PlaneDirection.Top:
                    return PlaneDirection.Bottom;
                case PlaneDirection.Right:
                    return PlaneDirection.Left;
                case PlaneDirection.Bottom:
                    return PlaneDirection.Top;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static bool IsHorizontal(this PlaneDirection direction)
        {
            return direction == PlaneDirection.Left || direction == PlaneDirection.Right;
        }
    }
}

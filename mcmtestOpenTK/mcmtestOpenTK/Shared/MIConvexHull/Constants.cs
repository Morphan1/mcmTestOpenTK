
namespace MIConvexHull
{
    internal static class Constants
    {
        internal const double epsilon = 1e-8;
        internal const double epsilonSquared = 1e-16;

        /// <summary>
        /// A value used to determine if a vertex lies on a plane.
        /// </summary>
        public const double PlaneDistanceTolerance = 0.0000001;
        // MCMONKEY - Transferred from ConvexHull.Constants.cs

        /// <summary>
        /// Checks whether to points are essentially the same position.
        /// </summary>
        /// <param name="pt1">The PT1.</param>
        /// <param name="pt2">The PT2.</param>
        /// <param name="dimension">The dimension.</param>
        /// <returns></returns>
        /*public static bool SamePosition(double[] pt1, double[] pt2, int dimension)
        {
            return (StarMath.norm2(StarMath.subtract(pt1, pt2, dimension), dimension, true) < epsilonSquared);
        }*/
        // MCMONKEY - Removed ~ StarMath isn't included, this function isn't referenced anyway.
    }
}

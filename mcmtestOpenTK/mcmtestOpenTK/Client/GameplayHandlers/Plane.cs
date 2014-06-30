using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GameplayHandlers
{
    public class Plane
    {
        /// <summary>
        /// The normal of the plane.
        /// </summary>
        public Location Normal;

        /// <summary>
        /// The first corner.
        /// </summary>
        public Location vec1;

        /// <summary>
        /// The second corner.
        /// </summary>
        public Location vec2;

        /// <summary>
        /// The third corner.
        /// </summary>
        public Location vec3;

        /// <summary>
        /// The distance from origin, in theory.
        /// </summary>
        public float D;

        public Plane(Location v1, Location v2, Location v3)
        {
            vec1 = v1;
            vec2 = v2;
            vec3 = v3;
            Normal = (v2 - v1).CrossProduct(v3 - v1).Normalize();
            D = -(Normal.Dot(vec1));
        }

        public Plane(Location v1, Location v2, Location v3, Location _normal)
        {
            vec1 = v1;
            vec2 = v2;
            vec3 = v3;
            Normal = _normal;
            D = -(Normal.Dot(vec1));
        }

        /// <summary>
        /// Finds where a line hits the line, if anywhere.
        /// </summary>
        /// <param name="start">The start of the line</param>
        /// <param name="end">The end of the line</param>
        /// <returns>A location of the hit, or NaN if none</returns>
        public Location IntersectLine(Location start, Location end)
        {
            Location ba = end - start;
            float nDotA = Normal.Dot(start);
            float nDotBA = Normal.Dot(ba);
            float t = -(nDotA + D) / (nDotBA);
            if (t < 0)
            {
                return Location.NaN;
            }
            return start + t * ba;
        }

        /// <summary>
        /// Returns the distance between a point and the plane.
        /// </summary>
        /// <param name="point">The point</param>
        /// <returns>The distance</returns>
        public float Distance(Location point)
        {
            return Normal.Dot(point) + D;
        }
    }
}

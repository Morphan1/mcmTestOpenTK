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
            /*
            Location ba = end - start;
            float nDotA = Normal.Dot(start);
            float nDotBA = Normal.Dot(ba);
            return start + (((D - nDotA) / nDotBA) * ba);
            */
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
    }
}

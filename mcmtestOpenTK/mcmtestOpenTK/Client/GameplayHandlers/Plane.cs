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
        public double D;

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
            double nDotA = Normal.Dot(start);
            double nDotBA = Normal.Dot(ba);
            double t = -(nDotA + D) / (nDotBA);
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
        public double Distance(Location point)
        {
            return Normal.Dot(point) + D;
        }

        /// <summary>
        /// Determines the signs of a box to the plane.
        /// If it returns 1, the box is above the plane.
        /// If it returns -1, the box is below the plane.
        /// If it returns 0, the box intersections with the plane.
        /// </summary>
        /// <param name="Mins">The mins of the box</param>
        /// <param name="Maxs">The maxes of the box</param>
        /// <returns>-1, 0, or 1</returns>
        public int SignToPlane(Location Mins, Location Maxs)
        {
            Location[] locs = new Location[8];
            locs[0] = new Location(Mins.X, Mins.Y, Mins.Z);
            locs[1] = new Location(Mins.X, Mins.Y, Maxs.Z);
            locs[2] = new Location(Mins.X, Maxs.Y, Mins.Z);
            locs[3] = new Location(Mins.X, Maxs.Y, Maxs.Z);
            locs[4] = new Location(Maxs.X, Mins.Y, Mins.Z);
            locs[5] = new Location(Maxs.X, Mins.Y, Maxs.Z);
            locs[6] = new Location(Maxs.X, Maxs.Y, Mins.Z);
            locs[7] = new Location(Maxs.X, Maxs.Y, Maxs.Z);
            int psign = Math.Sign(Distance(locs[0]));
            for (int i = 1; i < locs.Length; i++)
            {
                if (Math.Sign(Distance(locs[i])) != psign)
                {
                    return 0;
                }
            }
            return psign;
        }
    }
}

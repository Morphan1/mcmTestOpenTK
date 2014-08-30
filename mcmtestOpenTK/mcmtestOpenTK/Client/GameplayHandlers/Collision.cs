using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.GameplayHandlers
{
    public class Collision
    {
        public static Location Line(Location Start, Location Target, out Location normal)
        {
            // Watch the distance - we want the closest hit!
            double distance = (Target - Start).LengthSquared();
            // Keep track of what hit location we had
            Location final = Target;
            Location fnormal = Location.NaN;
            // Loop through all solids.
            for (int i = 0; i < MainGame.Solids.Count; i++)
            {
                // Get the current solid in the loop.
                Entity solid = MainGame.Solids[i];
                // Find where it hit
                Location tnormal;
                Location hit = solid.Closest(Start, Target, out tnormal);
                // NaN = no hit, ignore!
                if (hit.IsNaN())
                {
                    continue;
                }
                // Calculate how close it is.
                double newdist = (hit - Start).LengthSquared();
                // If the hit is closer than the previous hit
                if (newdist < distance)
                {
                    // Make this the new best hit
                    fnormal = tnormal;
                    distance = newdist;
                    final = hit;
                }
            }
            // Loops over, return whatever we got!
            normal = fnormal;
            return final;
        }

        /// <summary>
        /// Checks for solids at a specific point.
        /// </summary>
        /// <param name="point">The point to check at</param>
        /// <returns>Whether there is a solid there</returns>
        public static bool Point(Location point)
        {
            for (int i = 0; i < MainGame.Solids.Count; i++)
            {
                Entity solid = MainGame.Solids[i];
                if (solid.Solid)
                {
                    if (solid.Point(point))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks for solids within a specified bounding box.
        /// </summary>
        /// <param name="mins">The mins of the box</param>
        /// <param name="maxs">The maxes of the box</param>
        /// <returns>Whether there is a solid there</returns>
        public static bool Box(Location mins, Location maxs)
        {
            AABB Box2 = new AABB(mins, maxs);
            for (int i = 0; i < MainGame.Solids.Count; i++)
            {
                Entity solid = MainGame.Solids[i];
                if (solid.Solid)
                {
                    if (solid.Box(Box2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks for collision along a line and returns where it hit.
        /// </summary>
        /// <param name="Start">The start of the line</param>
        /// <param name="Target">The end of the line</param>
        /// <param name="Mins">The mins of the line's box</param>
        /// <param name="Maxs">The maxes of the line's box</param>
        /// <param name="hitnormal">The normal of the hit, or NaN if none</param>
        /// <returns>The end location of the linear movement</returns>
        public static Location LineBox(Location Start, Location Target, Location Mins, Location Maxs, out Location hitnormal)
        {
            if (Target == Start)
            {
                hitnormal = Location.NaN;
                return Target;
            }
            // Watch the distance - we want the closest hit!
            double distance = (Target - Start).LengthSquared();
            // Keep track of what hit location we had
            Location final = Target;
            Location fnormal = Location.NaN;
            AABB Box2 = new AABB(Mins, Maxs);
            // Loop through all solids.
            for (int i = 0; i < MainGame.Solids.Count; i++)
            {
                // Get the current solid in the loop.
                Entity solid = MainGame.Solids[i];
                if (solid.Solid)
                {
                    // Find where it here
                    Location normal;
                    Location hit = solid.ClosestBox(Box2, Start, Target, out normal);
                    // NaN = no hit, ignore!
                    if (hit.IsNaN())
                    {
                        continue;
                    }
                    // Calculate how close it is.
                    double newdist = (hit - Start).LengthSquared();
                    // If the hit is closer than the previous hit
                    if (newdist < distance)
                    {
                        // Make this the new best hit
                        distance = newdist;
                        fnormal = normal;
                        final = hit;
                    }
                }
            }
            // Loops over, return whatever we got!
            hitnormal = fnormal;
            return final;
        }

        /// <summary>
        /// Checks for collision along a line, and tries to slide past solids, and returns where it hit.
        /// </summary>
        /// <param name="Start">The start of the movement</param>
        /// <param name="Target">The end of the movement</param>
        /// <param name="Mins">The line's mins</param>
        /// <param name="Maxs">The line's maxes</param>
        /// <returns>Where the movement stopped</returns>
        public static Location SlideBox(Location Start, Location Target, Location Mins, Location Maxs)
        {
            Location Normal;
            Location current = Start;
            for (int i = 0; i < 3; i++)
            {
                Location pcurrent = current;
                current = LineBox(current, Target, Mins, Maxs, out Normal);
                if (Normal.IsNaN())
                {
                    return current;
                }
                if (current.IsNaN())
                {
                    return pcurrent;
                }
                // Jump back a tiny bit
                current += Normal * 0.0001f;
                /*
                if ((pcurrent - current).LengthSquared() > 0.000001f)
                {
                    current += (pcurrent - current).Normalize() * 0.0001f;
                }
                */
                if (Normal.X == 1 || Normal.X == -1)
                {
                    Target.X = current.X;
                }
                if (Normal.Y == 1 || Normal.Y == -1)
                {
                    Target.Y = current.Y;
                }
                if (Normal.Z == 1 || Normal.Z == -1)
                {
                    Target.Z = current.Z;
                }
            }
            return current;
        }
    }
}

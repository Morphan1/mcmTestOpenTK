using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GlobalHandler;

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
        /// Checks for solids at a specific point with the specific bounding box.
        /// </summary>
        /// <param name="point">The point to check at</param>
        /// <param name="mins">The mins of the box</param>
        /// <param name="maxs">The maxes of the box</param>
        /// <returns>Whether there is a solid there</returns>
        public static bool Box(Location point, Location mins, Location maxs)
        {
            AABB Box2 = new AABB(point, mins, maxs);
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
            AABB Box2 = new AABB(Location.Zero, Mins, Maxs);
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
                current = LineBox(current, Target, Mins, Maxs, out Normal);
                if (Normal.IsNaN())
                {
                    return current;
                }
                current += Normal * 0.0001f;
                //return current;
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

        /// <summary>
        /// Runs a collision check between two AABB objects.
        /// </summary>
        /// <param name="Position">The block's position</param>
        /// <param name="Mins">The block's mins</param>
        /// <param name="Maxs">The block's maxs</param>
        /// <param name="Mins2">The moving object's mins</param>
        /// <param name="Maxs2">The moving object's maxs</param>
        /// <param name="start">The starting location of the moving object</param>
        /// <param name="end">The ending location of the moving object</param>
        /// <param name="normal">The normal of the hit, or NaN if none</param>
        /// <returns>The location of the hit, or NaN if none</returns>
        public static Location AABBClosestBox(Location Position, Location Mins, Location Maxs, Location Mins2, Location Maxs2, Location start, Location end, out Location normal)
        {
            Location velocity = end - start;
            Location RealMins = Position + Mins;
            Location RealMaxs = Position + Maxs;
            Location RealMins2 = start + Mins2;
            Location RealMaxs2 = start + Maxs2;
            double xInvEntry, yInvEntry, zInvEntry;
            double xInvExit, yInvExit, zInvExit;
            if (end.X >= start.X)
            {
                xInvEntry = RealMins.X - RealMaxs2.X;
                xInvExit = RealMaxs.X - RealMins2.X;
            }
            else
            {
                xInvEntry = RealMaxs.X - RealMins2.X;
                xInvExit = RealMins.X - RealMaxs2.X;
            }
            if (end.Y >= start.Y)
            {
                yInvEntry = RealMins.Y - RealMaxs2.Y;
                yInvExit = RealMaxs.Y - RealMins2.Y;
            }
            else
            {
                yInvEntry = RealMaxs.Y - RealMins2.Y;
                yInvExit = RealMins.Y - RealMaxs2.Y;
            }
            if (end.Z >= start.Z)
            {
                zInvEntry = RealMins.Z - RealMaxs2.Z;
                zInvExit = RealMaxs.Z - RealMins2.Z;
            }
            else
            {
                zInvEntry = RealMaxs.Z - RealMins2.Z;
                zInvExit = RealMins.Z - RealMaxs2.Z;
            }
            double xEntry, yEntry, zEntry;
            double xExit, yExit, zExit;
            if (velocity.X == 0f)
            {
                xEntry = xInvEntry / 0.00000000000000000000000000000001f;
                xExit = xInvExit / 0.00000000000000000000000000000001f;
            }
            else
            {
                xEntry = xInvEntry / velocity.X;
                xExit = xInvExit / velocity.X;
            }
            if (velocity.Y == 0f)
            {
                yEntry = yInvEntry / 0.00000000000000000000000000000001f;
                yExit = yInvExit / 0.00000000000000000000000000000001f;
            }
            else
            {
                yEntry = yInvEntry / velocity.Y;
                yExit = yInvExit / velocity.Y;
            }
            if (velocity.Z == 0f)
            {
                zEntry = zInvEntry / 0.00000000000000000000000000000001f;
                zExit = zInvExit / 0.00000000000000000000000000000001f;
            }
            else
            {
                zEntry = zInvEntry / velocity.Z;
                zExit = zInvExit / velocity.Z;
            }
            double entryTime = Math.Max(Math.Max(xEntry, yEntry), zEntry);
            double exitTime = Math.Min(Math.Min(xExit, yExit), zExit);
            if (entryTime > exitTime || (xEntry < 0.0f && yEntry < 0.0f && zEntry < 0.0f) || xEntry > 1.0f || yEntry > 1.0f || zEntry > 1.0f)
            {
                normal = Location.NaN;
                return Location.NaN;
            }
            else
            {
                if (zEntry >= xEntry && zEntry >= yEntry)
                {
                    if (zInvEntry < 0)
                    {
                        normal = new Location(0, 0, 1);
                    }
                    else
                    {
                        normal = new Location(0, 0, -1);
                    }
                }
                else if (xEntry >= zEntry && xEntry >= yEntry)
                {
                    if (xInvEntry < 0)
                    {
                        normal = new Location(1, 0, 0);
                    }
                    else
                    {
                        normal = new Location(-1, 0, 0);
                    }
                }
                else
                {
                    if (yInvEntry < 0)
                    {
                        normal = new Location(0, 1, 0);
                    }
                    else
                    {
                        normal = new Location(0, -1, 0);
                    }
                }
                Location res = start + (end - start) * entryTime;
                return new Location(res.X, res.Y, res.Z);
            }
        }
    }
}

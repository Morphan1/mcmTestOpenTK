using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers
{
    class Collision
    {
        /// <summary>
        /// Returns whether there is something solid at the specified point.
        /// </summary>
        /// <param name="spot">The place to check</param>
        /// <returns>Whether there is something solid there</returns>
        public static bool Point(Location spot)
        {
            Entity ent;
            Location lower;
            Location upper;
            for (int i = 0; i < Server.MainWorld.Entities.Count; i++)
            {
                ent = Server.MainWorld.Entities[i];
                if (ent.Solid)
                {
                    lower = ent.Position + ent.Mins;
                    upper = ent.Position + ent.Maxs;
                    if (lower.X <= spot.X && lower.Y <= spot.Y && lower.Z <= spot.Z &&
                        upper.X >= spot.X && upper.Y >= spot.Y && upper.Z >= spot.Z)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether there is something solid inside the specified box.
        /// </summary>
        /// <param name="spot">The place to check</param>
        /// <param name="Mins">The minimum point of the collision box</param>
        /// <param name="Maxs">The maximum point of the collision box</param>
        /// <returns>Whether there is something solid there</returns>
        public static bool Box(Location spot, Location Mins, Location Maxs)
        {
            Entity ent;
            Location Low = spot + Mins;
            Location High = spot + Maxs;
            Location elow;
            Location ehigh;
            for (int i = 0; i < Server.MainWorld.Entities.Count; i++)
            {
                ent = Server.MainWorld.Entities[i];
                if (ent.Solid)
                {
                    elow = ent.Position + ent.Mins;
                    ehigh = ent.Position + ent.Maxs;
                    if (Low.X <= ehigh.X && Low.Y <= ehigh.Y && Low.Z <= ehigh.Z &&
                        High.X >= elow.X && High.Y >= elow.Y && High.Z >= elow.Z)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Slides along the collision map towards a target.
        /// </summary>
        /// <param name="Start">The starting location</param>
        /// <param name="Target">The target location</param>
        /// <param name="scale">The precision scale to use (for internal use)</param>
        /// <returns>where the collision slid to</returns>
        public static Location Slide(Location Start, Location Target, float scale = 1)
        {
            Location advance = Target - Start;
            double size = advance.Length() * scale;
            if (size == 0)
            {
                return Start;
            }
            advance /= size;
            int ticks = (int)Math.Floor(size);
            double extra = size - (double)ticks;
            ticks += 1;
            Location Nextpoint = Start;
            Location Jump;
            for (int i = 0; i < ticks; i++)
            {
                Jump = (i == ticks - 1 ? advance * extra : advance);
                Nextpoint += Jump;
                if (!Point(Nextpoint))
                {
                    continue;
                }
                // Try XY (no Z)
                Nextpoint.Z -= Jump.Z;
                if (!Point(Nextpoint))
                {
                    continue;
                }
                // Try XZ (no Y)
                Nextpoint.Z += Jump.Z;
                Nextpoint.Y -= Jump.Y;
                if (!Point(Nextpoint))
                {
                    continue;
                }
                // Try YZ (no X)
                Nextpoint.Y += Jump.Y;
                Nextpoint.X -= Jump.X;
                if (!Point(Nextpoint))
                {
                    continue;
                }
                // Try X (no YZ)
                Nextpoint.X += Jump.X;
                Nextpoint.Y -= Jump.Y;
                Nextpoint.Z -= Jump.Z;
                if (!Point(Nextpoint))
                {
                    continue;
                }
                // Try Y (no XZ)
                Nextpoint.Y += Jump.Y;
                Nextpoint.X -= Jump.X;
                if (!Point(Nextpoint))
                {
                    continue;
                }
                // Try Z (no XY)
                Nextpoint.Z += Jump.Z;
                Nextpoint.Y -= Jump.Y;
                if (!Point(Nextpoint))
                {
                    continue;
                }
                // Give up, do nothing!
                Nextpoint.Z -= Jump.Z;
                break;
            }
            if (scale == 1 || scale == 10 || scale == 100)
            {
                return Slide(Nextpoint, Target, scale * 10);
            }
            else
            {
                return Nextpoint;
            }
        }

        /// <summary>
        /// Slides along the collision map towards a target.
        /// </summary>
        /// <param name="Start">The starting location</param>
        /// <param name="Target">The target location</param>
        /// <param name="Mins">The lowest point of the collision box</param>
        /// <param name="Maxs">The highest point of the collision box</param>
        /// <param name="scale">The precision scale to use (for internal use)</param>
        /// <returns>where the collision slid to</returns>
        public static Location SlideBox(Location Start, Location Target, Location Mins, Location Maxs, float scale = 1)
        {
            Location advance = Target - Start;
            double size = advance.Length() * scale;
            if (size == 0)
            {
                return Start;
            }
            advance /= size;
            int ticks = (int)Math.Floor(size);
            double extra = size - (double)ticks;
            ticks += 1;
            Location Nextpoint = Start;
            Location Jump;
            for (int i = 0; i < ticks; i++)
            {
                Jump = (i == ticks - 1 ? advance * extra : advance);
                Nextpoint += Jump;
                if (!Box(Nextpoint, Mins, Maxs))
                {
                    continue;
                }
                // Try XY (no Z)
                Nextpoint.Z -= Jump.Z;
                if (!Box(Nextpoint, Mins, Maxs))
                {
                    continue;
                }
                // Try XZ (no Y)
                Nextpoint.Z += Jump.Z;
                Nextpoint.Y -= Jump.Y;
                if (!Box(Nextpoint, Mins, Maxs))
                {
                    continue;
                }
                // Try YZ (no X)
                Nextpoint.Y += Jump.Y;
                Nextpoint.X -= Jump.X;
                if (!Box(Nextpoint, Mins, Maxs))
                {
                    continue;
                }
                // Try X (no YZ)
                Nextpoint.X += Jump.X;
                Nextpoint.Y -= Jump.Y;
                Nextpoint.Z -= Jump.Z;
                if (!Box(Nextpoint, Mins, Maxs))
                {
                    continue;
                }
                // Try Y (no XZ)
                Nextpoint.Y += Jump.Y;
                Nextpoint.X -= Jump.X;
                if (!Box(Nextpoint, Mins, Maxs))
                {
                    continue;
                }
                // Try Z (no XY)
                Nextpoint.Z += Jump.Z;
                Nextpoint.Y -= Jump.Y;
                if (!Box(Nextpoint, Mins, Maxs))
                {
                    continue;
                }
                // Give up, do nothing!
                Nextpoint.Z -= Jump.Z;
                break;
            }
            if (scale == 1 || scale == 10 || scale == 100)
            {
                return SlideBox(Nextpoint, Target, Mins, Maxs, scale * 10);
            }
            else
            {
                return Nextpoint;
            }
        }

        /// <summary>
        /// Traces a straight line through the collision map, stopping immediately on collision.
        /// </summary>
        /// <param name="Start">The starting location</param>
        /// <param name="Target">The target location</param>
        /// <param name="scale">The precision scale to use (for internal use)</param>
        /// <returns>where the collision traced to</returns>
        public static Location Line(Location Start, Location Target, float scale = 1)
        {
            Location advance = Target - Start;
            double size = advance.Length() * scale;
            if (size == 0)
            {
                return Start;
            }
            advance /= size;
            int ticks = (int)Math.Floor(size);
            double extra = size - (double)ticks;
            ticks += 1;
            Location Nextpoint = Start;
            Location Jump;
            for (int i = 0; i < ticks; i++)
            {
                Jump = (i == ticks - 1 ? advance * extra : advance);
                Nextpoint += Jump;
                if (Point(Nextpoint))
                {
                    Nextpoint -= Jump;
                    break;
                }
            }
            if (scale == 1 || scale == 10 || scale == 100)
            {
                return Line(Nextpoint, Target, scale * 10);
            }
            else
            {
                return Nextpoint;
            }
        }

        /// <summary>
        /// Traces a straight line of boxes through the collision map, stopping immediately on collision.
        /// </summary>
        /// <param name="Start">The starting location</param>
        /// <param name="Target">The target location</param>
        /// <param name="scale">The precision scale to use (for internal use)</param>
        /// <returns>where the collision traced to</returns>
        public static Location LineBox(Location Start, Location Target, Location Mins, Location Maxs, float scale = 1)
        {
            Location advance = Target - Start;
            double size = advance.Length() * scale;
            if (size == 0)
            {
                return Start;
            }
            advance /= size;
            int ticks = (int)Math.Floor(size);
            double extra = size - (double)ticks;
            ticks += 1;
            Location Nextpoint = Start;
            Location Jump;
            for (int i = 0; i < ticks; i++)
            {
                Jump = (i == ticks - 1 ? advance * extra : advance);
                Nextpoint += Jump;
                if (Box(Nextpoint, Mins, Maxs))
                {
                    Nextpoint -= Jump;
                    break;
                }
            }
            if (scale == 1 || scale == 10 || scale == 100)
            {
                return LineBox(Nextpoint, Target, Mins, Maxs, scale * 10);
            }
            else
            {
                return Nextpoint;
            }
        }
    }
}

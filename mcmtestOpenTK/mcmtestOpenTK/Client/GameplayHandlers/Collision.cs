using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;

namespace mcmtestOpenTK.Client.GameplayHandlers
{
    public class Collision
    {
        /// <summary>
        /// Returns whether there is something solid at the specified point.
        /// </summary>
        /// <param name="spot">The place to check</param>
        /// <returns>Whether there is something solid there</returns>
        public static bool Point(Vector3 spot)
        {
            Entity ent;
            Vector3 lower;
            Vector3 upper;
            for (int i = 0; i < MainGame.Entities.Count; i++)
            {
                ent = MainGame.Entities[i];
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
        public static bool Box(Vector3 spot, Vector3 Mins, Vector3 Maxs)
        {
            Entity ent;
            Vector3 Low = spot + Mins;
            Vector3 High = spot + Maxs;
            Vector3 elow;
            Vector3 ehigh;
            for (int i = 0; i < MainGame.Entities.Count; i++)
            {
                ent = MainGame.Entities[i];
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
        /// <returns>where the collision slid to</returns>
        public static Vector3 Slide(Vector3 Start, Vector3 Target)
        {
            Vector3 advance = Target - Start;
            float size = advance.Length;
            if (size == 0)
            {
                return Start;
            }
            advance /= size;
            int ticks = (int)Math.Floor(size);
            float extra = size - (float)ticks;
            ticks += 1;
            Vector3 Nextpoint = Start;
            Vector3 Jump;
            for (int i = 0; i < ticks; i++)
            {
                Jump = (i == ticks - 1 ? advance * extra: advance);
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
            return Nextpoint;
        }

        /// <summary>
        /// Slides along the collision map towards a target.
        /// </summary>
        /// <param name="Start">The starting location</param>
        /// <param name="Target">The target location</param>
        /// <param name="Mins">The lowest point of the collision box</param>
        /// <param name="Maxs">The highest point of the collision box</param>
        /// <returns>where the collision slid to</returns>
        public static Vector3 SlideBox(Vector3 Start, Vector3 Target, Vector3 Mins, Vector3 Maxs)
        {
            Vector3 advance = Target - Start;
            float size = advance.Length;
            if (size == 0)
            {
                return Start;
            }
            advance /= size;
            int ticks = (int)Math.Floor(size);
            float extra = size - (float)ticks;
            ticks += 1;
            Vector3 Nextpoint = Start;
            Vector3 Jump;
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
            return Nextpoint;
        }

        /// <summary>
        /// Returns the advanced position of a moving entity, after collision is handled.
        /// </summary>
        /// <param name="Position">The starting location</param>
        /// <param name="target">The target location</param>
        /// <param name="mins">The lowest point of the collision box</param>
        /// <param name="maxs">The highest point of the collision box</param>
        /// <returns>Where the entity should end up</returns>
        public static Vector3 MoveForward(Vector3 Position, Vector3 target, Vector3 mins, Vector3 maxs)
        {
            return Collision.SlideBox(Position, target, mins, maxs);
        }
    }
}

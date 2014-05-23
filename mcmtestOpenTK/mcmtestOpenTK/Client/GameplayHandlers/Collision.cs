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
                        Console.WriteLine(spot + " hits the entity at " + ent.Position + " with colbox " + ent.Mins + ", " + ent.Maxs + " -- inside " + lower + ", " + upper);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the advanced position of a moving entity, after collision is handled.
        /// </summary>
        /// <param name="Position">The starting location</param>
        /// <param name="target">The target location</param>
        /// <returns>Where the entity should end up</returns>
        public static Vector3 MoveForward(Vector3 Position, Vector3 target)
        {
            if (!Collision.Point(target))
            {
                return target;
            }
            return Position;
        }
    }
}

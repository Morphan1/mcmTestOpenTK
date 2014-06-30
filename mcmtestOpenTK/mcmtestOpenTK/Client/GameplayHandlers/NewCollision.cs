using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GameplayHandlers
{
    public class NewCollision
    {
        public static Location Line(Location Start, Location Target)
        {
            // Watch the distance - we want the closest hit!
            double distance = (Target - Start).LengthSquared();
            // Keep track of what hit location we had
            Location final = Target;
            // Loop through all solids.
            for (int i = 0; i < MainGame.Solids.Count; i++)
            {
                // Get the current solid in the loop.
                Entity solid = MainGame.Solids[i];
                // Find where it here
                Location hit = solid.Closest(Start, Target);
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
                    final = hit;
                }
            }
            // Loops over, return whatever we got!
            return final;
        }

        public static Location LineBox(Location Start, Location Target, Location Mins, Location Maxs, out Location hitnormal)
        {
            if (Target == Start)
            {
                hitnormal = new Location(0, 0, 1);
                return Target;
            }
            // Watch the distance - we want the closest hit!
            double distance = (Target - Start).LengthSquared();
            // Keep track of what hit location we had
            Location final = Target;
            Location fnormal = (Start - Target).Normalize();
            // Loop through all solids.
            for (int i = 0; i < MainGame.Solids.Count; i++)
            {
                // Get the current solid in the loop.
                Entity solid = MainGame.Solids[i];
                // Find where it here
                Location normal;
                Location hit = solid.ClosestBox(Mins, Maxs, Start, Target, out normal);
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
            // Loops over, return whatever we got!
            hitnormal = fnormal;
            return final;
        }

        public static Location SlideBox(Location Start, Location Target, Location Mins, Location Maxs)
        {
            Location Normal;
            return LineBox(Start, Target, Mins, Maxs, out Normal) + Normal * 0.0001f;
        }
    }
}

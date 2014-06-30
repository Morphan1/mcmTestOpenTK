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
            Location movement = Target - Start;
            Location current = LineBox(Start, Start + movement, Mins, Maxs, out Normal) + Normal * 0.0001f;
            movement = Target - current;
            // TRY XY: NO Z
            movement.Z = 0;
            current = LineBox(current, current + movement, Mins, Maxs, out Normal) + Normal * 0.0001f;
            movement = Target - current;
            // TRY YZ: NO X
            movement.X = 0;
            current = LineBox(current, current + movement, Mins, Maxs, out Normal) + Normal * 0.0001f;
            movement = Target - current;
            // TRY XZ: NO Y
            movement.Y = 0;
            current = LineBox(current, current + movement, Mins, Maxs, out Normal) + Normal * 0.0001f;
            movement = Target - current;
            // TRY X: NO YZ
            movement.Y = 0;
            movement.Z = 0;
            current = LineBox(current, current + movement, Mins, Maxs, out Normal) + Normal * 0.0001f;
            movement = Target - current;
            // TRY Y: NO XZ
            movement.X = 0;
            movement.Z = 0;
            current = LineBox(current, current + movement, Mins, Maxs, out Normal) + Normal * 0.0001f;
            movement = Target - current;
            // TRY Z: NO XY
            movement.X = 0;
            movement.Y = 0;
            current = LineBox(current, current + movement, Mins, Maxs, out Normal) + Normal * 0.0001f;
            // We got what we got
            return current;
            /*
            Position = NewCollision.SlideBox(Position, target, new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));
            Position = NewCollision.SlideBox(Position, new Location(target.X, target.Y, Position.Z), new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));
            Position = NewCollision.SlideBox(Position, new Location(Position.X, target.Y, target.Z), new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));
            Position = NewCollision.SlideBox(Position, new Location(target.X, Position.Y, target.Z), new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));
            Position = NewCollision.SlideBox(Position, new Location(target.X, Position.Y, Position.Z), new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));
            Position = NewCollision.SlideBox(Position, new Location(Position.X, target.Y, Position.Z), new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));
            Position = NewCollision.SlideBox(Position, new Location(Position.X, Position.Y, target.Z), new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));*/
        }
    }
}

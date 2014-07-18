using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.Util;

namespace mcmtestOpenTK.Shared.Collision
{
    public class AABB
    {
        public Location Position;
        public Location Mins;
        public Location Maxs;

        public AABB(Location _Position, Location _Mins, Location _Maxs)
        {
            Position = _Position;
            Mins = _Mins;
            Maxs = _Maxs;
        }

        public void Include(Location point)
        {
            if (point.X < Mins.X)
            {
                Mins.X = point.X;
            }
            if (point.Y < Mins.Y)
            {
                Mins.Y = point.Y;
            }
            if (point.Z < Mins.Z)
            {
                Mins.Z = point.Z;
            }
            if (point.X > Maxs.X)
            {
                Maxs.X = point.X;
            }
            if (point.Y > Maxs.Y)
            {
                Maxs.Y = point.Y;
            }
            if (point.Z > Maxs.Z)
            {
                Maxs.Z = point.Z;
            }
        }

        public Plane[] CalculatePlanes()
        {
            Plane[] planes = new Plane[6];
            // Y-
            planes[0] = new Plane(Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z)/*, new Location(0, -1, 0)*/);
            // Y+
            planes[1] = new Plane(Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z)/*, new Location(0, 1, 0)*/);
            // X-
            planes[2] = new Plane(Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Maxs.Y, Maxs.Z)/*, new Location(-1, 0, 0)*/);
            // X+
            planes[3] = new Plane(Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z)/*, new Location(1, 0, 0)*/);
            // Z-
            planes[4] = new Plane(Position + new Location(Maxs.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z)/*, new Location(0, 0, -1)*/);
            // Z+
            planes[5] = new Plane(Position + new Location(Mins.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z)/*, new Location(0, 0, 1)*/);
            return planes;
        }

        public Plane[] CalculateTriangles()
        {
            Plane[] planes = new Plane[12];
            // Y-
            planes[0] = new Plane(Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z));
            planes[1] = new Plane(Position + new Location(Maxs.X, Mins.Y, Maxs.Z), Position + new Location(Mins.X, Mins.Y, Maxs.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z));
            // Y+
            planes[2] = new Plane(Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z));
            planes[3] = new Plane(Position + new Location(Mins.X, Maxs.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Mins.X, Maxs.Y, Mins.Z));
            // X-
            planes[4] = new Plane(Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Maxs.Y, Maxs.Z));
            planes[5] = new Plane(Position + new Location(Mins.X, Maxs.Y, Maxs.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Mins.Y, Maxs.Z));
            // X+
            planes[6] = new Plane(Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z));
            planes[7] = new Plane(Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z));
            // Z-
            planes[8] = new Plane(Position + new Location(Maxs.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z));
            planes[9] = new Plane(Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z));
            // Z+
            planes[10] = new Plane(Position + new Location(Mins.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z));
            planes[11] = new Plane(Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Mins.X, Maxs.Y, Maxs.Z), Position + new Location(Mins.X, Mins.Y, Maxs.Z));
            return planes;
        }

        public bool Point(Location spot)
        {
            Location lower = Position + Mins;
            Location upper = Position + Maxs;
            return lower.X <= spot.X && lower.Y <= spot.Y && lower.Z <= spot.Z &&
                upper.X >= spot.X && upper.Y >= spot.Y && upper.Z >= spot.Z;
        }

        public bool Box(AABB Box2)
        {
            Location elow = Position + Mins;
            Location ehigh = Position + Maxs;
            Location Low = Box2.Position + Box2.Mins;
            Location High = Box2.Position + Box2.Maxs;
            return Low.X <= ehigh.X && Low.Y <= ehigh.Y && Low.Z <= ehigh.Z &&
                        High.X >= elow.X && High.Y >= elow.Y && High.Z >= elow.Z;
        }

        public Location TraceLine(Location start, Location target, out Location normal)
        {
            Plane[] planes = CalculatePlanes();
            List<Plane> tplanes = new List<Plane>(3);
            if (start.X < Position.X + Mins.X)
            {
                tplanes.Add(planes[2]);
            }
            else if (start.X > Position.X + Maxs.X)
            {
                tplanes.Add(planes[3]);
            }
            if (start.Y < Position.Y + Mins.Y)
            {
                tplanes.Add(planes[0]);
            }
            else if (start.Y > Position.Y + Maxs.Y)
            {
                tplanes.Add(planes[1]);
            }
            if (start.Z < Position.Z + Mins.Z)
            {
                tplanes.Add(planes[4]);
            }
            else if (start.Z > Position.Z + Maxs.Z)
            {
                tplanes.Add(planes[5]);
            }
            return CollidePlanes(tplanes, start, target, out normal);
        }

        public Location CollidePlanes(List<Plane> planes, Location start, Location target, out Location normal)
        {
            double dist = (target - start).LengthSquared();
            for (int i = 0; i < planes.Count; i++)
            {
                Plane plane = planes[i];
                Location hit = plane.IntersectLine(start, target);
                if (!hit.IsNaN())
                {
                    double newdist = (hit - start).LengthSquared();
                    if (newdist < dist && Point(hit))
                    {
                        normal = plane.Normal;
                        return hit;
                    }
                }
            }
            normal = Location.NaN;
            return Location.NaN;
        }

        public Location TraceBox(AABB Box2, Location start, Location end, out Location normal)
        {
            return AABBClosestBox(Position, Mins, Maxs, Box2.Position + Box2.Mins, Box2.Position + Box2.Maxs, start, end, out normal);
        }

        public Location[] BoxPoints()
        {
            Location mins = Position + Mins;
            Location maxs = Position + Maxs;
            Location[] points = new Location[8];
            points[0] = new Location(mins.X, mins.Y, mins.Z);
            points[1] = new Location(mins.X, mins.Y, maxs.Z);
            points[2] = new Location(mins.X, maxs.Y, mins.Z);
            points[3] = new Location(mins.X, maxs.Y, maxs.Z);
            points[4] = new Location(maxs.X, mins.Y, mins.Z);
            points[5] = new Location(maxs.X, mins.Y, maxs.Z);
            points[6] = new Location(maxs.X, maxs.Y, mins.Z);
            points[7] = new Location(maxs.X, maxs.Y, maxs.Z);
            return points;
        }

        public Line[] BoxLines()
        {
            Line[] Lines = new Line[12];
            Lines[0] = new Line(new Location(Mins.X, Mins.Y, Mins.Z), new Location(Mins.X, Mins.Y, Maxs.Z));
            Lines[1] = new Line(new Location(Mins.X, Maxs.Y, Mins.Z), new Location(Mins.X, Maxs.Y, Maxs.Z));
            Lines[2] = new Line(new Location(Maxs.X, Mins.Y, Mins.Z), new Location(Maxs.X, Mins.Y, Maxs.Z));
            Lines[3] = new Line(new Location(Maxs.X, Maxs.Y, Mins.Z), new Location(Maxs.X, Maxs.Y, Maxs.Z));
            Lines[4] = new Line(new Location(Mins.X, Maxs.Y, Mins.Z), new Location(Maxs.X, Maxs.Y, Mins.Z));
            Lines[5] = new Line(new Location(Mins.X, Mins.Y, Mins.Z), new Location(Maxs.X, Mins.Y, Mins.Z));
            Lines[6] = new Line(new Location(Maxs.X, Mins.Y, Mins.Z), new Location(Maxs.X, Maxs.Y, Mins.Z));
            Lines[7] = new Line(new Location(Mins.X, Mins.Y, Mins.Z), new Location(Mins.X, Maxs.Y, Mins.Z));
            Lines[8] = new Line(new Location(Mins.X, Maxs.Y, Maxs.Z), new Location(Maxs.X, Maxs.Y, Maxs.Z));
            Lines[9] = new Line(new Location(Mins.X, Mins.Y, Maxs.Z), new Location(Maxs.X, Mins.Y, Maxs.Z));
            Lines[10] = new Line(new Location(Maxs.X, Mins.Y, Maxs.Z), new Location(Maxs.X, Maxs.Y, Maxs.Z));
            Lines[11] = new Line(new Location(Mins.X, Mins.Y, Maxs.Z), new Location(Mins.X, Maxs.Y, Maxs.Z));
            return Lines;
        }

        public override string ToString()
        {
            return "(BOX:" + Position + ",Min:" + Mins + ",Max:" + Maxs + ")";
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

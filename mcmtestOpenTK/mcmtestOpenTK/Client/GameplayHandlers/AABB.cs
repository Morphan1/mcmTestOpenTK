using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GameplayHandlers
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

        public RenderPlane[] CalculateTriangles()
        {
            RenderPlane[] planes = new RenderPlane[12];
            // Y-
            planes[0] = new RenderPlane(new Plane(Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z)));
            planes[1] = new RenderPlane(new Plane(Position + new Location(Maxs.X, Mins.Y, Maxs.Z), Position + new Location(Mins.X, Mins.Y, Maxs.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z)));
            // Y+
            planes[2] = new RenderPlane(new Plane(Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z)));
            planes[3] = new RenderPlane(new Plane(Position + new Location(Mins.X, Maxs.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Mins.X, Maxs.Y, Mins.Z)));
            // X-
            planes[4] = new RenderPlane(new Plane(Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Maxs.Y, Maxs.Z)));
            planes[5] = new RenderPlane(new Plane(Position + new Location(Mins.X, Maxs.Y, Maxs.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Mins.Y, Maxs.Z)));
            // X+
            planes[6] = new RenderPlane(new Plane(Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z)));
            planes[7] = new RenderPlane(new Plane(Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z)));
            // Z-
            planes[8] = new RenderPlane(new Plane(Position + new Location(Maxs.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Mins.Y, Mins.Z)));
            planes[9] = new RenderPlane(new Plane(Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z)));
            // Z+
            planes[10] = new RenderPlane(new Plane(Position + new Location(Mins.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z)));
            planes[11] = new RenderPlane(new Plane(Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), Position + new Location(Mins.X, Maxs.Y, Maxs.Z), Position + new Location(Mins.X, Mins.Y, Maxs.Z)));
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
            return Collision.AABBClosestBox(Position, Mins, Maxs, Box2.Position + Box2.Mins, Box2.Position + Box2.Maxs, start, end, out normal);
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
    }
}

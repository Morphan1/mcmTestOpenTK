using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared
{
    public class Minkowski
    {
        public static Minkowski From(List<Location> Vertices1, List<Location> Vertices2)
        {
            List<Location> Total = new List<Location>(Vertices1.Count * Vertices2.Count);
            for (int i = 0; i < Vertices1.Count; i++)
            {
                for (int x = 0; x < Vertices2.Count; x++)
                {
                    Total.Add(Vertices1[i] - Vertices2[x]);
                }
            }
            return new Minkowski(Total);
        }

        public List<Plane> Planes;

        public Minkowski(List<Location> vertices)
        {
            MIConvexHull.ConvexHull<Location, MIConvexHull.DefaultConvexFace<Location>> ch = MIConvexHull.ConvexHull.Create(vertices);
            Planes = new List<Plane>(vertices.Count);
            foreach (MIConvexHull.DefaultConvexFace<Location> face in ch.Faces)
            {
                Planes.Add(new Plane(face.Vertices[0], face.Vertices[1], face.Vertices[2]));
            }
        }

        public bool Point(Location point)
        {
            for (int i = 0; i < Planes.Count; i++)
            {
                int sign = Math.Sign(Planes[i].Distance(point));
                if (sign == 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Box(AABB Box2)
        {
            // Stupid brute force method
            // TODO: Replace with nice SAT method
            // Check if any points in the box are in the polygon: If so, collide!
            Location[] bpoints = Box2.BoxPoints();
            for (int i = 0; i < bpoints.Length; i++)
            {
                if (Point(bpoints[i]))
                {
                    return true;
                }
            }
            // Check if any points on the triangles are inside the box: If so, collide!
            for (int i = 0; i < Planes.Count; i++)
            {
                if (Box2.Point(Planes[i].vec1))
                {
                    return true;
                }
                if (Box2.Point(Planes[i].vec2))
                {
                    return true;
                }
                if (Box2.Point(Planes[i].vec3))
                {
                    return true;
                }
            }
            // Check if any of the edges of polygon ray-trace into the box: If so, collide!
            Location normal;
            Location hit;
            for (int i = 0; i < Planes.Count; i++)
            {
                // 1-2
                hit = Box2.TraceLine(Planes[i].vec1, Planes[i].vec2, out normal);
                if (!hit.IsNaN() && hit != Planes[i].vec2)
                {
                    return true;
                }
                // 2-3
                hit = Box2.TraceLine(Planes[i].vec2, Planes[i].vec3, out normal);
                if (!hit.IsNaN() && hit != Planes[i].vec3)
                {
                    return true;
                }
                // 3-1
                hit = Box2.TraceLine(Planes[i].vec3, Planes[i].vec1, out normal);
                if (!hit.IsNaN() && hit != Planes[i].vec1)
                {
                    return true;
                }
            }
            // Check if any of the edges of the box ray-trace into the polygon: If so, collide!
            // Irrelevant for our use case ~ would induce looping
            Line[] BoxLines = Box2.BoxLines();
            for (int i = 0; i < BoxLines.Length; i++)
            {
                if (!RayTrace(BoxLines[i].Start, BoxLines[i].End, out normal, false).IsNaN())
                {
                    return true;
                }
            }
            return false;
        }

        public Location RayTrace(Location start, Location target, out Location normal, bool box = true)
        {
            double dist = (target - start).LengthSquared();
            Location final = Location.NaN;
            Location fnormal = Location.NaN;
            for (int i = 0; i < Planes.Count; i++)
            {
                Plane plane = Planes[i];
                Location hit = plane.IntersectLine(start, target);
                if (!hit.IsNaN())
                {
                    double newdist = (hit - start).LengthSquared();
                    bool check = box ? Box(new AABB(hit, new Location(-1f), new Location(1f))) : Point(hit);
                    if (newdist < dist && check)
                    {
                        dist = newdist;
                        final = hit;
                        fnormal = plane.Normal;
                    }
                }
            }
            normal = fnormal;
            return final;
        }
    }
}

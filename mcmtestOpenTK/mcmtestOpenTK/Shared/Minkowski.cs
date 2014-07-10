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

        public Location RayTrace(Location target, out Location normal)
        {
            double dist = (target).LengthSquared();
            Location final = Location.NaN;
            Location fnormal = Location.NaN;
            for (int i = 0; i < Planes.Count; i++)
            {
                Plane plane = Planes[i];
                Location hit = plane.IntersectLine(Location.Zero, target);
                if (!hit.IsNaN())
                {
                    double newdist = hit.LengthSquared();
                    if (newdist < dist && Point(hit))
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

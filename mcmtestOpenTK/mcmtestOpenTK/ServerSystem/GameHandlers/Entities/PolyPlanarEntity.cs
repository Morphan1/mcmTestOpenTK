using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;
using mcmtestOpenTK.Shared.Game;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    class PolyPlanarEntity: Entity
    {
        List<Plane> Planes;
        List<string> Textures;
        public AABB BroadCollideBox;

        public PolyPlanarEntity()
            : base(false, true, EntityType.POLYPLANAR)
        {
            Planes = new List<Plane>();
            Textures = new List<string>();
            BroadCollideBox = new AABB(Location.Zero, Location.Zero, Location.Zero);
            Solid = true;
        }

        public override bool Box(AABB Box2)
        {
            if (!BroadCollideBox.Box(Box2))
            {
                return false;
            }
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
            Line[] BoxLines = Box2.BoxLines();
            for (int i = 0; i < BoxLines.Length; i++)
            {
                if (!Closest(BoxLines[i].Start, BoxLines[i].End, out normal).IsNaN())
                {
                    return true;
                }
            }
            return false;
        }

        public List<Location> Vertices()
        {
            List<Location> toret = new List<Location>(Planes.Count * 3);
            for (int i = 0; i < Planes.Count; i++)
            {
                if (!toret.Contains(Planes[i].vec1))
                {
                    toret.Add(Planes[i].vec1);
                }
                if (!toret.Contains(Planes[i].vec2))
                {
                    toret.Add(Planes[i].vec2);
                }
                if (!toret.Contains(Planes[i].vec3))
                {
                    toret.Add(Planes[i].vec3);
                }
            }
            return toret;
        }

        public override Location ClosestBox(AABB Box2, Location start, Location end, out Location normal)
        {
            Location hit = BroadCollideBox.TraceBox(Box2, start, end, out normal);
            AABB Box3 = new AABB(Box2.Position + start, Box2.Mins, Box2.Maxs);
            if (!hit.IsNaN() || BroadCollideBox.Box(Box3))
            {
                //return hit;
                Minkowski mink = Minkowski.From(Box3.BoxPoints().ToList(), Vertices());
                Location anormal;
                Location got = mink.RayTrace(Location.Zero, start - end, out anormal);
                if (!got.IsNaN())
                {
                    got = start - got;
                    anormal = -anormal;
                }
                normal = anormal;
                return got;
            }
            return Location.NaN;
        }

        public override Location Closest(Location start, Location target, out Location normal)
        {
            Location nor;
            if (!BroadCollideBox.TraceLine(start, target, out nor).IsNaN())
            {
                return CollidePlanes(start, target, out normal);
            }
            else
            {
                normal = Location.NaN;
                return Location.NaN;
            }
        }

        public Location CollidePlanes(Location start, Location target, out Location normal)
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
                    if (newdist < dist && Point(hit))
                    {
                        dist = newdist;
                        final = hit;
                        fnormal = plane.Normal;
                        //return hit;
                    }
                }
            }
            normal = fnormal;
            return final;
            //return Location.NaN;
        }

        public override List<Variable> GetSaveVars()
        {
            List<Variable> vars = base.GetSaveVars();
            StringBuilder planestr = new StringBuilder(Planes.Count * 36);
            for (int i = 0; i < Planes.Count; i++)
            {
                planestr.Append(Planes[i].ToString()).Append("_");
            }
            vars.Add(new Variable("planes", planestr.ToString()));
            return vars;
        }

        public override bool HandleVariable(string varname, string vardata)
        {
            if (varname == "planes")
            {
                string[] data = vardata.Split('_');
                Planes.Clear();
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].Length > 0)
                    {
                        Plane pl = Plane.FromString(data[i]);
                        pl = new Plane(Position + pl.vec1, Position + pl.vec2, Position + pl.vec3, pl.Normal);
                        Planes.Add(pl);
                    }
                }
                BroadCollideBox.Position = Planes[0].vec1;
                BroadCollideBox.Mins = Location.Zero;
                BroadCollideBox.Maxs = Location.Zero;
                for (int i = 0; i < Planes.Count; i++)
                {
                    //planestr.Append(Planes[i].ToString()).Append("_");
                    BroadCollideBox.Include(Planes[i].vec1);
                    BroadCollideBox.Include(Planes[i].vec2);
                    BroadCollideBox.Include(Planes[i].vec3);
                }
            }
            else if (varname == "texture")
            {
                Textures.Clear();
                string[] textures = vardata.Split('|');
                Textures.AddRange(textures);
            }
            else
            {
                return base.HandleVariable(varname, vardata);
            }
            return true;
        }

        public override byte[] GetData()
        {
            byte[] bytes = new byte[Planes.Count * (36 + 4)];
            for (int i = 0; i < Planes.Count; i++)
            {
                Planes[i].ToBytes().CopyTo(bytes, i * (36 + 4));
                Plane plane2 = Plane.FromBytes(Planes[i].ToBytes());
                BitConverter.GetBytes(NetStringManager.GetStringID(Textures[i])).CopyTo(bytes, (i + 1) * (36 + 4) - 4);
            }
            return bytes;
        }

        public override void Init()
        {
        }

        public override void Kill()
        {
        }

        public override bool Point(Location point)
        {
            return base.Point(point);
        }

        public override void Tick()
        {
        }
    }
}

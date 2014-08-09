using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.Networking;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;
using mcmtestOpenTK.Shared.Game;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class PolyPlanarEntity : Entity
    {
        public List<Plane> Planes;
        public List<Texture> Textures;

        public AABB BroadCollideBox;

        Model model;

        public PolyPlanarEntity()
            : base(false, EntityType.POLYPLANAR)
        {
            Solid = true;
            Planes = new List<Plane>();
            Textures = new List<Texture>();
            BroadCollideBox = new AABB(Location.Zero, Location.Zero, Location.Zero);
            model = Model.GetModel("test");
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

        Minkowski mink = null;

        Dictionary<AABB, Minkowski> Minkos = new Dictionary<AABB, Minkowski>();

        public Minkowski getMinko(AABB box)
        {
            //box.Mins += box.Position;
            //box.Maxs += box.Position;
            box.Position = Location.Zero;
            Minkowski mi;
            if (Minkos.TryGetValue(box, out mi))
            {
                return mi;
            }
            else
            {
                mi = Minkowski.From(Vertices(), box.BoxPoints().ToList());
                Minkos.Add(box, mi);
                return mi;
            }
        }

        public override Location ClosestBox(AABB Box2, Location start, Location end, out Location normal)
        {
            Location hit = BroadCollideBox.TraceBox(Box2, start, end, out normal);
            AABB Box3 = new AABB(Box2.Position + start, Box2.Mins, Box2.Maxs);
            if (!hit.IsNaN() || BroadCollideBox.Box(Box3))
            {
                mink = getMinko(Box2);
                Location anormal;
                Location got = mink.RayTrace(start, end, out anormal);
                if (!got.IsNaN())
                {
                    //got = start - got;
                    //anormal = -anormal;
                }
                normal = anormal;
                return got;
            }
            return Location.NaN;
        }

        public override bool Point(Location point)
        {
            if (!BroadCollideBox.Point(point))
            {
                return false;
            }
            for (int i = 0; i < Planes.Count; i++)
            {
                double dist = Planes[i].Distance(point);
                int sign = double.IsNaN(dist) ? 0: Math.Sign(dist);
                if (sign == 1)
                {
                    return false;
                }
            }
            return true;
        }

        public override void ReadBytes(byte[] data)
        {
            for (int i = 0; i < data.Length / (36 + 4); i++)
            {
                Planes.Add(new Plane(/*Position + */Location.FromBytes(data, i * (36 + 4)),
                    /*Position + */Location.FromBytes(data, i * (36 + 4) + 12), /*Position + */Location.FromBytes(data, i * (36 + 4) + 24)));
                Textures.Add(Texture.GetTexture(NetStringManager.GetStringForID(BitConverter.ToInt32(data, (i + 1) * (36 + 4) - 4))));
            }
            //StringBuilder planestr = new StringBuilder(Planes.Count * 36);
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

        public override void Tick()
        {
        }

        double yaw = 0;
        double scale = 1;
        int scalesign = 1;
        public override void Draw()
        {
            for (int i = 0; i < Planes.Count; i++)
            {
                Textures[i].Bind();
                SimpleRenderer.RenderPlane(Planes[i]);
            }
            yaw += MainGame.GraphicsDelta * 5;
            scale += MainGame.GraphicsDelta * scalesign;
            if (scale > 2 || scale < 1)
            {
                scalesign *= -1;
            }
            model.Draw(Position, new Location(yaw, 0, 0), new Location(scale));
            
            if (mink != null)
            {
                Texture.GetTexture("skylands/wall1").Bind();
                for (int i = 0; i < mink.Planes.Count; i++)
                {
                    SimpleRenderer.RenderPlane(mink.Planes[i]);
                }
                GL.Begin(PrimitiveType.Lines);
                GL.Color4(Color4.Green);
                GL.Vertex3(0f, 0f, 0f);
                GL.Vertex3(mink.Planes[0].vec1.X, mink.Planes[0].vec1.Y, mink.Planes[0].vec1.Z);
                GL.End();
            }
            
            //SimpleRenderer.RenderAABB(BroadCollideBox);
        }
    }
}

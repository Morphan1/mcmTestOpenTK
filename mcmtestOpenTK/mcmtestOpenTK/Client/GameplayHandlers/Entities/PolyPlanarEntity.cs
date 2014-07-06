using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.Networking;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class PolyPlanarEntity: Entity
    {
        public List<RenderPlane> Planes;
        public List<Texture> Textures;

        public PolyPlanarEntity()
            : base(false, EntityType.POLYPLANAR)
        {
            Solid = true;
            Planes = new List<RenderPlane>();
            Textures = new List<Texture>();
        }

        public override bool Box(Location mins, Location maxs)
        {
            return base.Box(mins, maxs);
        }

        public override Location ClosestBox(Location mins, Location maxs, Location start, Location end, out Location normal)
        {
            return base.ClosestBox(mins, maxs, start, end, out normal);
        }

        public override bool Point(Location point)
        {
            for (int i = 0; i < Planes.Count; i++)
            {
                int sign = Math.Sign(Planes[i].Internal.Distance(point));
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
                Planes.Add(new RenderPlane(new Plane(Location.FromBytes(data, i * (36 + 4)),
                    Location.FromBytes(data, i * (36 + 4) + 12), Location.FromBytes(data, i * (36 + 4) + 24))));
                Textures.Add(Texture.GetTexture(NetStringManager.GetStringForID(BitConverter.ToInt32(data, (i + 1) * (36 + 4) - 4))));
            }
            StringBuilder planestr = new StringBuilder(Planes.Count * 36);
            for (int i = 0; i < Planes.Count; i++)
            {
                planestr.Append(Planes[i].Internal.ToString()).Append("_");
            }
        }

        public override Location Closest(Location start, Location target, out Location normal)
        {
            return CollidePlanes(start, target, out normal);
        }


        public Location CollidePlanes(Location start, Location target, out Location normal)
        {
            double dist = (target - start).LengthSquared();
            Location final = Location.NaN;
            Location fnormal = Location.NaN;
            for (int i = 0; i < Planes.Count; i++)
            {
                Plane plane = Planes[i].Internal;
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

        public override void Draw()
        {
            for (int i = 0; i < Planes.Count; i++)
            {
                Textures[i].Bind();
                Planes[i].Draw();
            }
        }
    }
}

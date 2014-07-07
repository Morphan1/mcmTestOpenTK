﻿using System;
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

        public override bool Box(AABB Box2)
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
            // Check if any points on the triangle are inside the box: If so, collide!
            for (int i = 0; i < Planes.Count; i++)
            {
                if (Box2.Point(Planes[i].Internal.vec1))
                {
                    return true;
                }
                if (Box2.Point(Planes[i].Internal.vec2))
                {
                    return true;
                }
                if (Box2.Point(Planes[i].Internal.vec3))
                {
                    return true;
                }
            }
            // Check if any of the edges of polygon ray-trace into the box: If so, collide!
            Location normal;
            /*
            for (int i = 0; i < Planes.Count; i++)
            {
                // 1-2
                if (!Box2.TraceLine(Planes[i].Internal.vec1, Planes[i].Internal.vec2, out normal).IsNaN())
                {
                    return true;
                }
                // 2-3
                if (!Box2.TraceLine(Planes[i].Internal.vec2, Planes[i].Internal.vec3, out normal).IsNaN())
                {
                    return true;
                }
                // 3-1
                if (!Box2.TraceLine(Planes[i].Internal.vec3, Planes[i].Internal.vec1, out normal).IsNaN())
                {
                    return true;
                }
            }
            */
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

        public override Location ClosestBox(AABB Box2, Location start, Location end, out Location normal)
        {
            Location movevec = end - start;
            Location movnrm = movevec.Normalize();
            AABB Box3 = new AABB(start, Box2.Mins, Box2.Maxs);
            Location[] bpoints = Box3.BoxPoints();
            Location final = Location.NaN;
            Location fnormal = Location.NaN;
            double dist = movevec.Length();
            for (int i = 0; i < bpoints.Length; i++)
            {
                Location cnormal;
                Location hit = Closest(bpoints[i], bpoints[i] + movevec, out cnormal);
                if (!hit.IsNaN())
                {
                    double newdist = (hit - start).Length();
                    if (newdist < dist/* && Box(new AABB(Location.Zero, Box3.Mins + movnrm * newdist, Box3.Maxs + movnrm * newdist))*/)
                    {
                        dist = newdist;
                        final = hit;
                        fnormal = cnormal;
                    }
                }
            }
            normal = fnormal;
            return final;
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
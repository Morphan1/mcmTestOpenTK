using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    class CubeEntity : Entity
    {
        /// <summary>
        /// The render model this cube uses.
        /// </summary>
        public CubeModel model;

        public CubeEntity()
            : base(false, EntityType.CUBE)
        {
            model = new CubeModel(Position, Location.One, null);
            Mins = new Location(0);
            Solid = true;
        }

        /// <summary>
        /// Do not call: This entity does not tick!
        /// </summary>
        public override void Tick()
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            model.Position = Position;
            model.Draw();
        }

        public override void ReadBytes(byte[] data)
        {
            if (data.Length < 12 + 4 * 4 + 1)
            {
                throw new ArgumentException("Binary network data for CUBE entity is invalid!");
            }
            int pos = 0;
            model.Scale = Location.FromBytes(data, pos);
            pos += 12;
            Maxs = model.Scale;
            model.Texture_HScale = BitConverter.ToSingle(data, pos);
            pos += 4;
            model.Texture_VScale = BitConverter.ToSingle(data, pos);
            pos += 4;
            model.Texture_HShift = BitConverter.ToSingle(data, pos);
            pos += 4;
            model.Texture_VShift = BitConverter.ToSingle(data, pos);
            pos += 4;
            string texture = NetStringManager.GetStringForID(BitConverter.ToInt32(data, pos));
            model.texture = Texture.GetTexture(texture);
            pos += 4;
        }

        public override bool Point(Location spot)
        {
            Location lower = Position + Mins;
            Location upper = Position + Maxs;
            return lower.X <= spot.X && lower.Y <= spot.Y && lower.Z <= spot.Z &&
                upper.X >= spot.X && upper.Y >= spot.Y && upper.Z >= spot.Z;
        }

        public override bool Box(Location Low, Location High)
        {
            Location elow = Position + Mins;
            Location ehigh = Position + Maxs;
            return Low.X <= ehigh.X && Low.Y <= ehigh.Y && Low.Z <= ehigh.Z &&
                        High.X >= elow.X && High.Y >= elow.Y && High.Z >= elow.Z;
        }

        /*
        public Plane[] CalculatePlanes()
        {
            Plane[] planes = new Plane[6];
            // Y-
            planes[0] = new Plane(Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z), new Location(0, -1, 0));
            // Y+
            planes[1] = new Plane(Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), new Location(0, 1, 0));
            // X-
            planes[2] = new Plane(Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Mins.X, Maxs.Y, Mins.Z), Position + new Location(Mins.X, Maxs.Y, Maxs.Z), new Location(-1, 0, 0));
            // X+
            planes[3] = new Plane(Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), new Location(1, 0, 0));
            // Z-
            planes[4] = new Plane(Position + new Location(Mins.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Mins.Y, Mins.Z), Position + new Location(Maxs.X, Maxs.Y, Mins.Z), new Location(0, 0, -1));
            // Z+
            planes[5] = new Plane(Position + new Location(Mins.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Mins.Y, Maxs.Z), Position + new Location(Maxs.X, Maxs.Y, Maxs.Z), new Location(0, 0, 1));
            return planes;
        }

        public override Location Closest(Location start, Location target)
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
            return CollidePlanes(tplanes, start, target);
        }
        */
        /*
        public Location CollidePlanes(List<Plane> planes, Location start, Location target)
        {
            //float dist = (target - start).LengthSquared();
            //Location final = Location.NaN;
            for (int i = 0; i < planes.Count; i++)
            {
                Plane plane = planes[i];
                Location hit = plane.IntersectLine(start, target);
                if (!hit.IsNaN())
                {
                    //float newdist = (hit - start).LengthSquared();
                    if (/*newdist < dist && * /Point(hit))
                    {
                        //dist = newdist;
                        //final = hit;
                        return hit;
                    }
                }
            }
            //return final;
            return Location.NaN;
        }
        */

        public override Location ClosestBox(Location Mins2, Location Maxs2, Location start, Location end, out Location normal)
        {
            return Collision.AABBClosestBox(Position, Mins, Maxs, Mins2, Maxs2, start, end, out normal);
        }
    }
}

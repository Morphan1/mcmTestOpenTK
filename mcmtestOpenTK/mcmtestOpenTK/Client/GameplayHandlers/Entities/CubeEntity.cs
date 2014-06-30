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
            : base(false)
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

        public bool Point(Location spot)
        {
            Location lower = Position + Mins;
            Location upper = Position + Maxs;
            return lower.X <= spot.X && lower.Y <= spot.Y && lower.Z <= spot.Z &&
                upper.X >= spot.X && upper.Y >= spot.Y && upper.Z >= spot.Z;
        }

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
                    if (/*newdist < dist && */Point(hit))
                    {
                        //dist = newdist;
                        //final = hit;
                        SysConsole.Output(OutputType.INFO, "DISTANCE FROM HIT TO PLANE: " + plane.Distance(hit));
                        SysConsole.Output(OutputType.INFO, "DISTANCE FROM PLAYER TO PLANE: " + plane.Distance(Player.player.Position));
                        return hit;
                    }
                }
            }
            //return final;
            return Location.NaN;
        }

        public override Location ClosestBox(Location Mins2, Location Maxs2, Location start, Location end, out Location normal)
        {
            Location velocity = end - start;
            Box b1 = new Box()
            {
                x = start.X + Mins2.X,
                y = start.Y + Mins2.Y,
                z = start.Z + Mins2.Z,
                w = Maxs2.X - Mins2.X,
                h = Maxs2.Y - Mins2.Y,
                d = Maxs2.Z - Mins2.Z,
                vx = velocity.X,
                vy = velocity.Y,
                vz = velocity.Z
            };
            Box b2 = new Box()
            {
                x = Position.X + Mins.X,
                y = Position.Y + Mins.Y,
                z = Position.Z + Mins.Z,
                w = Maxs.X - Mins.X,
                h = Maxs.Y - Mins.Y,
                d = Maxs.Z - Mins.Z,
                vx = 0,
                vy = 0,
                vz = 0
            };
            float xInvEntry, yInvEntry, zInvEntry;
            float xInvExit, yInvExit, zInvExit;
            if (b1.vx > 0.0f)
            {
                xInvEntry = b2.x - (b1.x + b1.w);
                xInvExit = (b2.x + b2.w) - b1.x;
            }
            else
            {
                xInvEntry = (b2.x + b2.w) - b1.x;
                xInvExit = b2.x - (b1.x + b1.w);
            }
            if (b1.vy > 0.0f)
            {
                yInvEntry = b2.y - (b1.y + b1.h);
                yInvExit = (b2.y + b2.h) - b1.y;
            }
            else
            {
                yInvEntry = (b2.y + b2.h) - b1.y;
                yInvExit = b2.y - (b1.y + b1.h);
            }
            if (b1.vz > 0.0f)
            {
                zInvEntry = b2.z - (b1.z + b1.d);
                zInvExit = (b2.z + b2.d) - b1.z;
            }
            else
            {
                zInvEntry = (b2.z + b2.d) - b1.z;
                zInvExit = b2.z - (b1.z + b1.d);
            }
            float xEntry, yEntry, zEntry;
            float xExit, yExit, zExit;
            if (b1.vx == 0.0f)
            {
                xEntry = float.NegativeInfinity;
                xExit = float.PositiveInfinity;
            }
            else
            {
                xEntry = xInvEntry / b1.vx;
                xExit = xInvExit / b1.vx;
            }
            if (b1.vy == 0.0f)
            {
                yEntry = float.NegativeInfinity;
                yExit = float.PositiveInfinity;
            }
            else
            {
                yEntry = yInvEntry / b1.vy;
                yExit = yInvExit / b1.vy;
            }
            if (b1.vz == 0.0f)
            {
                zEntry = float.NegativeInfinity;
                zExit = float.PositiveInfinity;
            }
            else
            {
                zEntry = zInvEntry / b1.vz;
                zExit = zInvExit / b1.vz;
            }
            float entryTime = Math.Max(Math.Max(xEntry, yEntry), zEntry);
            float exitTime = Math.Min(Math.Min(xExit, yExit), zExit);
            if (entryTime > exitTime || xEntry < 0.0f && yEntry < 0.0f && zEntry < 0.0f || xEntry > 1.0f || yEntry > 1.0f || zEntry > 1.0f)
            {
                normal = Location.NaN;
                return Location.NaN;
            }
            else
            {
                float normalx, normaly, normalz;
                if (xEntry > yEntry)
                {
                    if (xInvEntry < 0.0f)
                    {
                        normalx = 1.0f;
                        normaly = 0.0f;
                    }
                    else
                    {
                        normalx = -1.0f;
                        normaly = 0.0f;
                    }
                }
                else
                {
                    if (yInvEntry < 0.0f)
                    {
                        normalx = 0.0f;
                        normaly = 1.0f;
                    }
                    else
                    {
                        normalx = 0.0f;
                        normaly = -1.0f;
                    }
                }
                // TODO: CALCULATE NORMAL Z
                normalz = 0;
                normal = new Location(normalx, normaly, normalz);
                Location res = start + (end - start) * entryTime;
                return new Location(res.X, res.Y, res.Z);
            }
        }
    }
    public class Box
    {
        public float x, y, z;
        public float w, h, d;
        public float vx, vy, vz;
    }
}

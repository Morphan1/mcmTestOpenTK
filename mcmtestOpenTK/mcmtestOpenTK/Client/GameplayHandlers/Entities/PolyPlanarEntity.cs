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
            return base.Point(point);
        }

        public override void ReadBytes(byte[] data)
        {
            for (int i = 0; i < data.Length / (36 + 4); i++)
            {
                Planes.Add(new RenderPlane(new Plane(Location.FromBytes(data, i * (36 + 4)),
                    Location.FromBytes(data, i * (36 + 4) + 12), Location.FromBytes(data, i * (36 + 4) + 24))));
                Textures.Add(Texture.GetTexture(NetStringManager.GetStringForID(BitConverter.ToInt32(data, (i + 1) * (36 + 4) - 4))));
                //Textures.Add(Texture.GetTexture("skylands/wall" + Utilities.random.Next(4)));
            }
            StringBuilder planestr = new StringBuilder(Planes.Count * 36);
            for (int i = 0; i < Planes.Count; i++)
            {
                planestr.Append(Planes[i].Internal.ToString()).Append("_");
            }
            FileHandler.AppendText("test.log", "planes: " + planestr.ToString() + "\n\n");
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

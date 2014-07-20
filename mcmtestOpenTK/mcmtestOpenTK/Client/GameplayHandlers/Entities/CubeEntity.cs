using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;
using mcmtestOpenTK.Shared.Game;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    class CubeEntity : Entity
    {
        public AABB CollisionModel;

        public Texture texture;
        public Shader shader = null;
        public float Texture_HScale;
        public float Texture_VScale;
        public float Texture_HShift;
        public float Texture_VShift;

        public CubeEntity()
            : base(false, EntityType.CUBE)
        {
            Mins = new Location(0);
            Solid = true;
            CollisionModel = new AABB(Position, Mins, Maxs);
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
            if (shader != null)
            {
                shader.Bind();
            }
            else
            {
                MainGame.GeneralShader.Bind();
            }
            texture.Bind();
            CollisionModel.Position = Position;
            Plane[] tris = CollisionModel.CalculateTriangles();
            for (int i = 0; i < tris.Length; i++)
            {
                SimpleRenderer.RenderPlane(tris[i]);
            }
        }

        public override void ReadBytes(byte[] data)
        {
            if (data.Length < 12 + 4 * 4 + 1)
            {
                throw new ArgumentException("Binary network data for CUBE entity is invalid!");
            }
            int pos = 0;
            Maxs = Location.FromBytes(data, pos);
            pos += 12;
            Texture_HScale = BitConverter.ToSingle(data, pos);
            pos += 4;
            Texture_VScale = BitConverter.ToSingle(data, pos);
            pos += 4;
            Texture_HShift = BitConverter.ToSingle(data, pos);
            pos += 4;
            Texture_VShift = BitConverter.ToSingle(data, pos);
            pos += 4;
            string texturestr = NetStringManager.GetStringForID(BitConverter.ToInt32(data, pos));
            texture = Texture.GetTexture(texturestr);
            pos += 4;
            CollisionModel.Position = Position;
            CollisionModel.Mins = Mins;
            CollisionModel.Maxs = Maxs;
            /*
            RenderPlane[] Planes = CollisionModel.CalculateTriangles();
            StringBuilder planestr = new StringBuilder(Planes.Length * 36);
            for (int i = 0; i < Planes.Length; i++)
            {
                planestr.Append(Planes[i].Internal.ToString()).Append("_");
            }
            FileHandler.AppendText("test.log", "planes: " + planestr.ToString() + "\n\n");
            */
        }

        public override bool Point(Location spot)
        {
            return CollisionModel.Point(spot);
        }

        public override bool Box(AABB Box2)
        {
            return CollisionModel.Box(Box2);
        }

        public override Location Closest(Location start, Location target, out Location normal)
        {
            return CollisionModel.TraceLine(start, target, out normal);
        }

        public override Location ClosestBox(AABB Box2, Location start, Location end, out Location normal)
        {
            return CollisionModel.TraceBox(Box2, start, end, out normal);
        }
    }
}

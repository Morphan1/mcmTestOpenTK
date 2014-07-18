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
            if (model.shader != null)
            {
                model.shader.Bind();
            }
            else
            {
                MainGame.GeneralShader.Bind();
            }
            model.texture.Bind();
            CollisionModel.Position = Position;
            Plane[] tris = CollisionModel.CalculateTriangles();
            for (int i = 0; i < tris.Length; i++)
            {
                new RenderPlane(tris[i]).Draw();
            }
            
            model.Position = Position;
           // model.Draw();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using OpenTK;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    class CubeEntity: Entity
    {
        /// <summary>
        /// The render model this cube uses.
        /// </summary>
        public CubeModel model;

        public CubeEntity(): base(false)
        {
            model = new CubeModel(Position, Vector3.One, null);
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
            if (data.Length < 13)
            {
                throw new ArgumentException("Binary network data for CUBE entity is invalid!");
            }
            float X = BitConverter.ToSingle(data, 0);
            float Y = BitConverter.ToSingle(data, 4);
            float Z = BitConverter.ToSingle(data, 8);
            model.Scale = new Vector3(X, Y, Z);
            UIConsole.WriteLine("Cube at " + Position.ToString() + ", scale: " + model.Scale.ToString());
            string texture = FileHandler.encoding.GetString(data, 12, data.Length - 12);
            model.texture = Texture.GetTexture(texture);
        }
    }
}

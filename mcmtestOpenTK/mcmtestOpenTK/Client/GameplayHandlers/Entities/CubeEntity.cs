using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
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
            if (data.Length < 13)
            {
                throw new ArgumentException("Binary network data for CUBE entity is invalid!");
            }
            model.Scale = Location.FromBytes(data, 0);
            Maxs = model.Scale;
            string texture = FileHandler.encoding.GetString(data, 12, data.Length - 12);
            model.texture = Texture.GetTexture(texture);
        }
    }
}

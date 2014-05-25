using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    class Bullet: MovingEntity
    {
        CubeModel model;

        public Bullet(): base()
        {
            model = new CubeModel(Position, new Location(1), Texture.Console);
            Mins = new Location(-0.5f);
            Maxs = new Location(0.5f);
            CheckCollision = true;
        }

        public override void ReadBytes(byte[] data)
        {
            if (data.Length != 24)
            {
                throw new ArgumentException("Binary network data for CUBE entity is invalid!");
            }
            Direction = Location.FromBytes(data, 0);
            Velocity = Location.FromBytes(data, 12);
        }

        public override void Draw()
        {
            model.Position = Position;
            model.Angle = Direction.X;
            model.Draw();
        }
    }
}

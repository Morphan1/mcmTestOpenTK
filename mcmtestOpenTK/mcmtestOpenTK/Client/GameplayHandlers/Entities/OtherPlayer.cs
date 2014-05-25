using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class OtherPlayer: MovingEntity
    {
        CubeModel model;

        public OtherPlayer(): base()
        {
            model = new CubeModel(Location.Zero, new Location(3, 3, 8), Texture.Test);
            Mins = new Location(-1.5f, -1.5f, 0);
            Maxs = new Location(1.5f, 1.5f, 8);
            Solid = false;
            CheckCollision = true;
            Gravity = 100;
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void Draw()
        {
            model.Position = new Location(Position.X - 1.5f, Position.Y - 1.5f, Position.Z);
            model.Draw();
        }

        public override void ReadBytes(byte[] data)
        {
            if (data.Length > 0)
            {
                throw new ArgumentException("Binary network data was unexpected!");
            }
        }
    }
}

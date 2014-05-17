using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class OtherPlayer: MovingEntity
    {
        public override void Tick()
        {
            base.Tick();
        }

        public override void Draw()
        {
            MainGame.DrawCube(Position.X, Position.Y, Position.Z, Direction.X);
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

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
        /// <summary>
        /// The default collision mins for a player.
        /// </summary>
        public static Location DefaultMins = new Location(-1.5f, -1.5f, 0);

        /// <summary>
        /// The default collision maxes for a player.
        /// </summary>
        public static Location DefaultMaxes = new Location(1.5f, 1.5f, 8f);

        CubeModel model;

        public OtherPlayer(): base()
        {
            model = new CubeModel(Location.Zero, new Location(3, 3, 8), Texture.Test);
            Mins = DefaultMins;
            Maxs = DefaultMaxes;
            Solid = true;
            CheckCollision = true;
            MoveType = MovementType.SlideBox;
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

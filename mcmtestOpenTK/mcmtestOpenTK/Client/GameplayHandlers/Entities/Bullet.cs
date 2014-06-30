using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    class Bullet: MovingEntity
    {
        public CubeModel model;

        public int LifeTicks = 1000;

        public Texture texture = Texture.Console;

        public Location start;

        public Bullet(): base()
        {
            model = new CubeModel(Position, new Location(1), Texture.Console);
            Mins = new Location(-0.5f);
            Maxs = new Location(0.5f);
            CheckCollision = true;
            Solid = false;
        }

        public override void Tick()
        {
            LifeTicks--;
            base.Tick();
            if (LifeTicks <= 0)
            {
                IsValid = false;
            }
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
            model.texture = texture;
            model.Position = Position;
            model.Angle = Direction.X;
            model.Draw();
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(Color4.Green);
            GL.Vertex3(Position.X, Position.Y, Position.Z);
            GL.Vertex3(start.X, start.Y, start.Z);
            GL.End();
        }
    }
}

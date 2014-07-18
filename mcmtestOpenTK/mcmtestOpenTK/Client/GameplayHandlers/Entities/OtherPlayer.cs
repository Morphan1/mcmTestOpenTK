using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsIn;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;
using mcmtestOpenTK.Shared.Game;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class OtherPlayer: Entity
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

        /// <summary>
        /// The player's gravity.
        /// </summary>
        public float Gravity = 100;

        /// <summary>
        /// The player's movement velocity.
        /// </summary>
        public Location Velocity;

        /// <summary>
        /// What direction this place is facing.
        /// </summary>
        public Location Direction;

        public bool Forward = false;
        public bool Back = false;
        public bool Left = false;
        public bool Right = false;
        public bool Up = false;
        public bool Down = false;
        public bool Slow = false;
        public bool Jumped = false;

        public bool Noclip = false;

        public const double MoveSpeed = 35;
        public const double JumpPower = 50;
        public const double AirSpeedMult = 0.05f;

        public OtherPlayer(): base(true, EntityType.PLAYER)
        {
            model = new CubeModel(Location.Zero, new Location(3, 3, 8), Texture.Test);
            Mins = DefaultMins;
            Maxs = DefaultMaxes;
            Solid = true;
            Gravity = 100;
            PacketsToApply = new List<PlayerPositionPacketIn>();
            LastPacket = new PlayerPositionPacketIn();
            LastPacket.Time = MainGame.GlobalTickTime;
            LastMovement = MainGame.GlobalTickTime;
        }

        public override void Tick()
        {
            Tick(MainGame.Delta, false);
        }

        public List<PlayerPositionPacketIn> PacketsToApply;

        public void Tick(double MyDelta, bool isCustom)
        {
            if (MyDelta == 0)
            {
                return;
            }
            if (!isCustom)
            {
                int count = PacketsToApply.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        PacketsToApply[i].Execute();
                    }
                    PacketsToApply.RemoveRange(0, count);
                }
            }
            Location movement = Location.Zero;
            if (Left)
            {
                movement.Y = -1;
            }
            if (Right)
            {
                movement.Y = 1;
            }
            if (Back)
            {
                if (movement.Y != 0)
                {
                    movement.Y *= 0.5f;
                    movement.X = 0.5f;
                }
                else
                {
                    movement.X = 1;
                }
            }
            if (Forward)
            {
                if (movement.Y != 0)
                {
                    movement.Y *= 0.5f;
                    movement.X = -0.5f;
                }
                else
                {
                    movement.X = -1;
                }
            }
            if (Down)
            {
                Maxs = new Location(1.5f, 1.5f, 5);
            }
            else
            {
                if (!Collision.Box(Position, new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8)))
                {
                    Maxs = new Location(1.5f, 1.5f, 8);
                }
                else
                {
                    Down = true;
                }
            }
            if (Noclip)
            {
                Gravity = 0;
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180, Direction.Y * Utilities.PI180);
                }
                if (Up)
                {
                    movement.Z = 1;
                }
                if (Down)
                {
                    movement.Z -= 1;
                }
                Velocity = movement * 30;
            }
            else
            {
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180);
                }
                bool on_ground = Velocity.Z < 0.01f && Collision.Box(Position, new Location(-1.5f, -1.5f, -0.01f), new Location(1.5f, 1.5f, 2));
                if (Up && on_ground && !Jumped)
                {
                    Velocity.Z = JumpPower * (Down ? 0.5 : 1);
                    Jumped = true;
                }
                else if (!Up && Jumped)
                {
                    Jumped = false;
                }
                Velocity.X += ((movement.X * MoveSpeed * (Slow || Down ? 0.5 : 1)) - Velocity.X) * MyDelta * 8 * (on_ground ? 1 : AirSpeedMult);
                Velocity.Y += ((movement.Y * MoveSpeed * (Slow || Down ? 0.5 : 1)) - Velocity.Y) * MyDelta * 8 * (on_ground ? 1 : AirSpeedMult);
                Velocity.Z -= Gravity * MyDelta;
            }
            Location ploc = Position;
            Location target = Position + Velocity * MyDelta;
            Position = Collision.SlideBox(Position, target, new Location(-1.5f, -1.5f, 0), Maxs);
            Velocity = (Position - ploc) / MyDelta;
            // Climb steps
            if (Position != target) // If we missed the target
            {
                // Try a flat target
                target = new Location(target.X, target.Y, Position.Z);
                // If the flat target is solid
                if (Collision.Box(target, new Location(-1.5f, -1.5f, 0), Maxs))
                {
                    // Raise the target by 2
                    target.Z += 2;
                    // If the higher target has room
                    if (!Collision.Box(target, new Location(-1.5f, -1.5f, 0), Maxs))
                    {
                        // Move up and forward
                        Position = Collision.SlideBox(Position + new Location(0, 0, 2), target + new Location(0, 0, 2), new Location(-1.5f, -1.5f, 0), Maxs);
                        // move back into place
                        Position = Collision.SlideBox(Position, target + new Location(0, 0, -2), new Location(-1.5f, -1.5f, 0), Maxs);
                    }
                }
            }
            if (!isCustom)
            {
                LastTick = MainGame.GlobalTickTime;
            }
        }

        public double LastTick;
        public double LastMovement;
        Location LastMoveLoc;
        Location LastVelocity;
        public bool LastJumped = false;
        PlayerPositionPacketIn LastPacket;

        public void ApplyNewMovement(double MoveTime, PlayerPositionPacketIn pack)
        {
            bool WasSolid = Solid;
            Solid = false;
            // Apply last known position / movement.
            Position = LastMoveLoc;
            Velocity = LastVelocity;
            Jumped = LastJumped;
            PlayerPositionPacketIn.ApplyPosition(this, LastPacket.movement, LastPacket.direction.X, LastPacket.direction.Y);
            // Tick from last known movement to new position.
            double targetdelta = MoveTime - LastMovement;
            while (targetdelta > 1d / 60d)
            {
                Tick(1d / 60d, true);
                targetdelta -= 1d / 60d;
            }
            Tick(targetdelta, true);
            // Apply the real position the player was at when the packet was sent.
            Position = pack.position;
            Velocity = pack.velocity;
            Direction = pack.direction;
            LastMoveLoc = Position;
            LastVelocity = Velocity;
            LastMovement = MoveTime;
            LastJumped = Jumped;
            LastPacket = pack;
            // Apply the new movement packet.
            PlayerPositionPacketIn.ApplyPosition(this, pack.movement, pack.direction.X, pack.direction.Y);
            // Tick back up to now.
            targetdelta = (float)(LastTick - MoveTime);
            while (targetdelta > 1d / 60d)
            {
                Tick(1d / 60d, true);
                targetdelta -= 1d / 60d;
            }
            Tick(targetdelta, true);
            Solid = WasSolid;
        }

        public override void Draw()
        {
            model.Position = new Location(Position.X - 1.5f, Position.Y - 1.5f, Position.Z);
            model.Draw();
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(Color4.Green);
            GL.Vertex3(Position.X, Position.Y, Position.Z);
            GL.Vertex3(Player.player.Position.X, Player.player.Position.Y, Player.player.Position.Z);
            GL.End();
        }

        public override void ReadBytes(byte[] data)
        {
            if (data.Length > 0)
            {
                Noclip = data[0] == 1;
            }
        }
    }
}

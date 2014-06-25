using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Input;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GameplayHandlers
{
    public class Player
    {
        /// <summary>
        /// The default player - there's only ever one!
        /// </summary>
        public static Player player;

        /// <summary>
        /// The precise X/Y/Z location of the entity.
        /// </summary>
        public Location Position = Location.Zero;

        /// <summary>
        /// How much health the player currently has.
        /// </summary>
        public float Health = 0;

        /// <summary>
        /// The precise X/Y/Z worldly movement speed.
        /// </summary>
        public Location Velocity = Location.Zero;

        /// <summary>
        /// The precise Yaw/Pitch/Roll direction of the entity.
        /// </summary>
        public Location Direction = Location.Zero;

        float ticker = 0;

        /// <summary>
        /// Called to tick the default player.
        /// </summary>
        public void Update()
        {
            // Don't move if not even spawned.
            if (!MainGame.Spawned)
            {
                if (MouseHandler.MouseCaptured)
                {
                    MouseHandler.ReleaseMouse();
                }
                return;
            }
            // Mouse based rotation
            Direction.X += MouseHandler.MouseDelta.X;
            Direction.Y += MouseHandler.MouseDelta.Y;
            while (Direction.X < 0)
            {
                Direction.X += 360;
            }
            while (Direction.X > 360)
            {
                Direction.X -= 360;
            }
            if (Direction.Y > 80)
            {
                Direction.Y = 80;
            }
            if (Direction.Y < -80)
            {
                Direction.Y = -80;
            }
            MainGame.Forward = Utilities.ForwardVector(Direction.X * Utilities.PI180, Direction.Y * Utilities.PI180);
            // TODO: Switch to 'capturemouse' command.
            if (KeyHandler.IsPressed(Key.LShift))
            {
                if (MouseHandler.MouseCaptured)
                {
                    MouseHandler.ReleaseMouse();
                }
                else
                {
                    MouseHandler.CaptureMouse();
                }
            }
            // Keyboard based movement.
            Location oldvel = Velocity;
            Location movement = Location.Zero;
            bool left = KeyHandler.KeyBindIsDown(KeyBind.LEFT);
            bool right = KeyHandler.KeyBindIsDown(KeyBind.RIGHT);
            bool forward = KeyHandler.KeyBindIsDown(KeyBind.FORWARD);
            bool back = KeyHandler.KeyBindIsDown(KeyBind.BACK);
            bool up = KeyHandler.KeyBindIsDown(KeyBind.UP);
            bool down = KeyHandler.KeyBindIsDown(KeyBind.DOWN);
            if (left)
            {
                movement.Y = -1;
            }
            if (right)
            {
                movement.Y = 1;
            }
            if (back)
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
            if (forward)
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
            if (ClientCVar.g_noclip.ValueB)
            {
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180, Direction.Y * Utilities.PI180);
                }
                if (up)
                {
                    movement.Z = 1;
                }
                if (down)
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
                if (down)
                {
                    Velocity.Z = 0;
                    Position.Z = 20;
                }
                if (up)
                {
                    if (Velocity.Z < 0.1f && Velocity.Z > -0.1f
                        && Collision.Box(Position, new Location(-1.5f, -1.5f, -0.5f), new Location(1.5f, 1.5f, 2)))
                    {
                        Velocity.Z = 20;
                    }
                }
                Velocity = new Location(movement.X * 30, movement.Y * 30, Velocity.Z);
                Velocity.Z -= 100 * MainGame.DeltaF;
            }
            Location target = Position + (oldvel + Velocity) * 0.5f * MainGame.DeltaF;
            float pZ = Position.Z;
            Position = Collision.SlideBox(Position, target, new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));
            if (!ClientCVar.g_noclip.ValueB)
            {
                Velocity.Z = (Position.Z - pZ) / MainGame.DeltaF;
            }
            byte move = MovementPacketOut.GetControlByte(forward, back, left, right, up, down);
            if (move != lastMove || Direction != lastdir)
            {
                lastMove = move;
                lastdir = Location.Zero;
                if (NetworkBase.IsActive)
                {
                    NetworkBase.Send(new MovementPacketOut(MainGame.GlobalTickTime, move, Direction.X, Direction.Y));
                }
            }
        }

        byte lastMove = 0;
        Location lastdir = Location.Zero;
    }
}

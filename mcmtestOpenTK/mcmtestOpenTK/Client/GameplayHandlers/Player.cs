using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Client.Networking.PacketsOut;

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
        public Vector3 Position = Vector3.Zero;

        /// <summary>
        /// How much health the player currently has.
        /// </summary>
        public float Health = 0;

        /// <summary>
        /// The precise X/Y/Z worldly movement speed.
        /// </summary>
        public Vector3 Velocity = Vector3.Zero;

        /// <summary>
        /// The precise Yaw/Pitch/Roll direction of the entity.
        /// </summary>
        public Vector3 Angle = Vector3.Zero;

        float ticker = 0;

        /// <summary>
        /// Called to tick the default player.
        /// </summary>
        public void Update()
        {
            // Mouse based rotation
            Angle.X += MouseHandler.MouseDelta.X;
            Angle.Y += MouseHandler.MouseDelta.Y;
            while (Angle.X < 0)
            {
                Angle.X += 360;
            }
            while (Angle.X > 360)
            {
                Angle.X -= 360;
            }
            if (Angle.Y > 80)
            {
                Angle.Y = 80;
            }
            if (Angle.Y < -80)
            {
                Angle.Y = -80;
            }
            MainGame.Forward = Util.ForwardVector(MathHelper.DegreesToRadians(Angle.X), MathHelper.DegreesToRadians(Angle.Y));
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
            Vector3 movement = Vector3.Zero;
            bool left = KeyHandler.IsDown(Key.D);
            bool right = KeyHandler.IsDown(Key.A);
            bool forward = KeyHandler.IsDown(Key.W);
            bool back = KeyHandler.IsDown(Key.S);
            bool up = KeyHandler.IsDown(Key.Space);
            bool down = KeyHandler.IsDown(Key.C);
            if (left && !right)
            {
                movement.Y = 1;
            }
            if (right && !left)
            {
                movement.Y = -1;
            }
            if (forward && !back)
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
            if (back && !forward)
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
            if (movement.LengthSquared > 0)
            {
                movement = Util.RotateVector(movement, MathHelper.DegreesToRadians(Angle.X), MathHelper.DegreesToRadians(Angle.Y));
            }
            if (up)
            {
                movement.Z = 1;
            }
            if (down)
            {
                movement.Z -= 1;
            }
            Velocity = new Vector3(movement * 30);
            Position += Velocity * MainGame.DeltaF;
            ticker += MainGame.DeltaF;
            // TODO: Have server identify proper TPS
            if (ticker > (1 / 20) && NetworkBase.IsActive)
            {
                ticker = 0;
                if (Position != LastLoc)
                {
                    NetworkBase.Send(new PositionPacketOut(Position));
                    LastLoc = Position;
                }
            }
        }

        Vector3 LastLoc = Vector3.Zero;
    }
}

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

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class Player : Entity
    {
        /// <summary>
        /// The default player - there's only ever one!
        /// </summary>
        public static Player player;

        /// <summary>
        /// Called to tick the default player.
        /// </summary>
        public override void Update()
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
                    Console.WriteLine("Release mouse!");
                }
                else
                {
                    MouseHandler.CaptureMouse();
                    Console.WriteLine("Capture mouse!");
                }
            }
            // Keyboard based movement.
            Vector3 movement = Vector3.Zero;
            bool left = KeyHandler.IsDown(Key.D);
            bool right = KeyHandler.IsDown(Key.A);
            bool forward = KeyHandler.IsDown(Key.W);
            bool back = KeyHandler.IsDown(Key.S);
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
            Velocity = new Vector3(movement * 30);

            // Standard entity updating
            base.Update();
        }

        /// <summary>
        /// Does nothing (empty override).
        /// </summary>
        public override void Draw()
        {
            // No player drawing, global drawing will handle this!
        }
    }
}

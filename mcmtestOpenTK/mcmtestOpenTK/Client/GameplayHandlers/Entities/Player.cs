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

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class Player : Breakable
    {
        /// <summary>
        /// The default player - there's only ever one!
        /// </summary>
        public static Player player;
        public static int CenterX = 0;
        static int CenterY = 0;

        /// <summary>
        /// Called to tick the default player.
        /// </summary>
        public override void Update()
        {
            // Mouse based rotation
            Point center = MainGame.PrimaryGameWindow.PointToScreen(new Point(MainGame.ScreenWidth / 2, MainGame.ScreenHeight / 2));
            Angle.X += (float)(((MainGame.ScreenWidth / 2) - MainGame.PrimaryGameWindow.Mouse.X) * MainGame.Delta * MainGame.MouseSensitivity);
            Angle.Y += (float)(((MainGame.ScreenHeight / 2) - MainGame.PrimaryGameWindow.Mouse.Y) * MainGame.Delta * MainGame.MouseSensitivity);
            Mouse.SetPosition(center.X, center.Y);
            CenterX = center.X;
            CenterY = center.Y;
            MainGame.Forward = Util.ForwardVector(MathHelper.DegreesToRadians(Angle.X), MathHelper.DegreesToRadians(Angle.Y));

            // Keyboard based movement.
            Vector2 movement = Vector2.Zero;
            bool left = MainGame.CurrentKeyboard.IsKeyDown(Key.D);
            bool right = MainGame.CurrentKeyboard.IsKeyDown(Key.A);
            bool forward = MainGame.CurrentKeyboard.IsKeyDown(Key.W);
            bool back = MainGame.CurrentKeyboard.IsKeyDown(Key.S);
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
                movement = Util.RotateVector(movement, MathHelper.DegreesToRadians(Angle.X));
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GameplayHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking.OneOffs;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Client.Networking;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_Game: AbstractScreen
    {
        public Screen_Game(): base(ScreenMode.Game)
        {
        }

        public override void SwitchTo()
        {
        }

        public override void Init()
        {
            debug = new PieceOfText("", new Location(5, MainGame.ScreenHeight / 5 * 3, 0));
            Crosshair.texture = Texture.GetTexture("common/crosshair");
            Initted = true;
        }

        /// <summary>
        /// The crosshair's render square.
        /// </summary>
        public static Square Crosshair = new Square();

        // Temporary, for testing
        public static int X = 0;
        public static int Y = 0;
        public static PieceOfText debug;

        public override void Tick()
        {
            // Update gameplay
            X = MainGame.PrimaryGameWindow.Mouse.X;
            Y = MainGame.PrimaryGameWindow.Mouse.Y;

            // Update player
            Player.player.Update(MainGame.DeltaF, false);

            // Update world
            MainGame.TickWorld();

            // Debug stuff, always near end
            debug.Text = TextStyle.Color_Readable +
                "\ncFPS: " + MainGame.cFPS +
                "\ngFPS: " + MainGame.gFPS +
                "\nPosition: " + Player.player.Position.ToString() +
                "\nDirection: " + Player.player.Direction.ToString() +
                "\nVelocity: " + Player.player.Velocity.ToString() +
                "\nNow: " + Utilities.DateTimeToString(DateTime.Now) +
                "\nPing: " + (int)(NetworkBase.Ping * 1000);

            if (MouseHandler.CurrentMouse.IsButtonDown(MouseButton.Left) && !MouseHandler.PreviousMouse.IsButtonDown(MouseButton.Left))
            {
                MainGame.SpawnEntity(new Bullet()
                {
                    Position = NewCollision.Line(Player.player.Position + new Location(0, 0, 6), Player.player.Position + new Location(0, 0, 6) + (MainGame.Forward * 200)),
                    LifeTicks = 60 * 20
                });
                MainGame.SpawnEntity(new Bullet()
                {
                    Position = Player.player.Position + new Location(0, 0, 6) + (MainGame.Forward * 200),
                    texture = Texture.White,
                    LifeTicks = 60 * 20
                });
            }
        }

        public override void Draw2D()
        {
            // Correct the crosshair
            float chsize = 16 * ClientCVar.r_crosshairscale.ValueF;
            Crosshair.PositionLow = new Location(MainGame.ScreenWidth / 2 - chsize, MainGame.ScreenHeight / 2 - chsize, 0);
            Crosshair.PositionHigh = new Location(MainGame.ScreenWidth / 2 + chsize, MainGame.ScreenHeight / 2 + chsize, 0);
            // Draw crosshair
            Crosshair.Draw();

            // Render debug text
            FontSet.DrawColoredText(debug);
        }

        public override void Draw3D()
        {
            // Correct clear color in case 3D isn't rendered blockingly
            GL.ClearColor(new Color4(0, 0, 0, 255));

            // Draw everything in the world
            MainGame.DrawWorld();
        }
    }
}
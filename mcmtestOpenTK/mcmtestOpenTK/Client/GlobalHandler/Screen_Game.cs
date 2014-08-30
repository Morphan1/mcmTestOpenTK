using System;
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
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;
using mcmtestOpenTK.Client.Networking.PacketsIn;

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

        public override void SwitchFrom()
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
            Player.player.Update(MainGame.Delta, false);

            // Update world
            MainGame.TickWorld();

            // Debug stuff, always near end
            debug.Text = TextStyle.Color_Readable +
                "\ncFPS: " + MainGame.cFPS +
                "\ngFPS: " + MainGame.gFPS +
                "\nPosition: " + Player.player.Position.ToString() +
                "\nDirection: " + Player.player.Direction.ToString() +
                "\nVelocity: " + Math.Round(Math.Sqrt(Player.player.Velocity.X * Player.player.Velocity.X + Player.player.Velocity.Y * Player.player.Velocity.Y)) + ", " + Player.player.Velocity.ToString() +
                "\nNow: " + Utilities.DateTimeToString(DateTime.Now) +
                "\nPing: " + (int)(NetworkBase.Ping * 1000) +
                "\nItem: " + GiveItemPacketIn.LastItem +
                "\nOn Ground: " + Player.player.onground;

            if (MouseHandler.CurrentMouse.IsButtonDown(MouseButton.Left) && !MouseHandler.PreviousMouse.IsButtonDown(MouseButton.Left))
            {
                Location normal;
                Location hit = Collision.LineBox(Player.player.Position + new Location(0, 0, 12),
                    Player.player.Position + new Location(0, 0, 12) + (MainGame.Forward * 200), new Location(-3, -3, 0), Player.player.Maxs, out normal);
                SysConsole.Output(OutputType.INFO, "Hit at " + hit);
                MainGame.SpawnEntity(new Bullet()
                {
                    Position = hit,
                    LifeTicks = 60 * 20,
                    start = hit + (normal.IsNaN() ? Location.Zero : normal * 20)
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
            // Draw everything in the world
            MainGame.DrawWorld();

            // Draw the player's held item
            if (GiveItemPacketIn.LastItem != null)
            {
                Location Forward = Utilities.ForwardVector((Player.player.Direction.X - 20) * Utilities.PI180, 0) * 2f;
                GiveItemPacketIn.LastItem.Draw(Player.player.Position + new Location(0, 0, 4.5f)
                    + Forward, new Location(Player.player.Direction.X, -30 + (Player.player.Direction.Y / 2), 0));
            }
        }
    }
}

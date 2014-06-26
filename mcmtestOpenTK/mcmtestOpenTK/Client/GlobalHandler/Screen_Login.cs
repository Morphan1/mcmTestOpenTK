using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using mcmtestOpenTK.Client.UIHandlers.Menus;
using mcmtestOpenTK.Client.UIHandlers.Menus.Login;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_Login : AbstractScreen
    {
        public MenuSystem Menus;

        public LoginButton LoginB;

        public TextBox UsernameBox;

        public TextBox PasswordBox;

        public Screen_Login(): base(ScreenMode.Login)
        {
        }

        public override void SwitchTo()
        {
            // Clear and redo the whole menu system.
            Init();
        }

        public override void Init()
        {
            // Prepare menu system
            Menus = new MenuSystem();
            Menus.Init();
            // Create menu items
            LoginB = new LoginButton(5, 50);
            PlayOfflineButton PlayOffB = new PlayOfflineButton(5, 50);
            MenuLabel UsernameLabel = new MenuLabel("Username:", 5, 50);
            MenuLabel PasswordLabel = new MenuLabel("Password:", 5, 50);
            UsernameBox = new LoginBox(MainGame.ScreenWidth / 2 - 260, 50, Texture.GetTexture("menus/textbox_back"), 500);
            PasswordBox = new PasswordBox(MainGame.ScreenWidth / 2 - 260, 50, Texture.GetTexture("menus/textbox_back"), 500);
            PasswordBox.Password = true;
            // Calculate widths for X-centering
            float lwidth = LoginB.RenderSquare.PositionHigh.X - LoginB.RenderSquare.PositionLow.X + 10;
            float pwidth = PlayOffB.RenderSquare.PositionHigh.X - PlayOffB.RenderSquare.PositionLow.X;
            float ladjust = (MainGame.ScreenWidth / 2 - (lwidth + pwidth) / 2) - LoginB.RenderSquare.PositionLow.Y;
            // Adjust their X locations (centering)
            LoginB.RenderSquare.PositionLow.X += ladjust;
            LoginB.RenderSquare.PositionHigh.X += ladjust;
            PlayOffB.RenderSquare.PositionLow.X += ladjust + lwidth;
            PlayOffB.RenderSquare.PositionHigh.X += ladjust + lwidth;
            ladjust = (MainGame.ScreenWidth / 2 -
                (LoginB.RenderSquare.PositionHigh.X - LoginB.RenderSquare.PositionLow.X)) - LoginB.RenderSquare.PositionLow.X;
            ladjust = UsernameBox.RenderSquare.PositionLow.X - UsernameLabel.RenderSquare.PositionLow.X;
            UsernameLabel.RenderSquare.PositionLow.X += ladjust;
            UsernameLabel.RenderSquare.PositionHigh.X += ladjust;
            ladjust = PasswordBox.RenderSquare.PositionLow.X - PasswordLabel.RenderSquare.PositionLow.X;
            PasswordLabel.RenderSquare.PositionLow.X += ladjust;
            PasswordLabel.RenderSquare.PositionHigh.X += ladjust;
            // Calculate heights for Y-centering
            float uheight = UsernameBox.RenderSquare.PositionHigh.Y - UsernameBox.RenderSquare.PositionLow.Y + 10;
            float ulheight = UsernameLabel.RenderSquare.PositionHigh.Y - UsernameLabel.RenderSquare.PositionLow.Y + 10;
            float pheight = PasswordBox.RenderSquare.PositionHigh.Y - PasswordBox.RenderSquare.PositionLow.Y + 10;
            float plheight = PasswordLabel.RenderSquare.PositionHigh.Y - PasswordLabel.RenderSquare.PositionLow.Y + 10;
            float lheight = LoginB.RenderSquare.PositionHigh.Y - LoginB.RenderSquare.PositionLow.Y + 10;
            float theight = uheight + pheight + lheight + ulheight + plheight;
            ladjust = (MainGame.ScreenHeight / 2 - theight / 2) - UsernameBox.RenderSquare.PositionLow.Y;
            // Adjust their Y position (centering)
            UsernameLabel.RenderSquare.PositionLow.Y += ladjust;
            UsernameLabel.RenderSquare.PositionHigh.Y += ladjust;
            UsernameBox.RenderSquare.PositionLow.Y += ladjust + ulheight;
            UsernameBox.RenderSquare.PositionHigh.Y += ladjust + ulheight;
            PasswordLabel.RenderSquare.PositionLow.Y += ladjust + ulheight + uheight;
            PasswordLabel.RenderSquare.PositionHigh.Y += ladjust + ulheight + uheight;
            PasswordBox.RenderSquare.PositionLow.Y += ladjust + uheight + ulheight + plheight;
            PasswordBox.RenderSquare.PositionHigh.Y += ladjust + uheight + ulheight + plheight;
            LoginB.RenderSquare.PositionLow.Y += ladjust + uheight + pheight + ulheight + plheight;
            LoginB.RenderSquare.PositionHigh.Y += ladjust + uheight + pheight + ulheight + plheight;
            PlayOffB.RenderSquare.PositionLow.Y += ladjust + uheight + pheight + ulheight + plheight;
            PlayOffB.RenderSquare.PositionHigh.Y += ladjust + uheight + pheight + ulheight + plheight;
            // Add items to the menu
            Menus.Add(UsernameLabel);
            Menus.Add(UsernameBox);
            Menus.Add(PasswordLabel);
            Menus.Add(PasswordBox);
            Menus.Add(LoginB);
            Menus.Add(PlayOffB);
            // Done!
            Initted = true;
        }

        float cyan = 0;

        float cyanmod = 1;

        public override void Tick()
        {
            cyan += (float)MainGame.Delta * cyanmod / 5;
            if (cyan >= 1)
            {
                cyanmod = -1;
                cyan = 1;
            }
            if (cyan <= 0)
            {
                cyanmod = 1;
                cyan = 0;
            }
            Menus.Tick();
            KeyHandler.GetKBState();
        }

        public override void Draw2D()
        {
            GL.ClearColor(new Color4(0, (byte)(cyan * 255), (byte)(cyan * 255), 255));
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Menus.Draw();
        }

        public override void Draw3D()
        {
        }
    }
}

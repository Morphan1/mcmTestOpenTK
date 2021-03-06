﻿using System;
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
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_Login : AbstractScreen
    {
        public Screen_Login(): base(ScreenMode.Login)
        {
        }

        public MenuSystem Menus;

        public override void SwitchTo()
        {
            // Recalculate all items
            Recalc();
            Menus.Recalculate();
        }

        public override void SwitchFrom()
        {
            ClientCVar.u_login_username.Set("");
            ClientCVar.u_login_password.Set("");
            // Remove any notice
            Menus.Notice = null;
        }

        TextBox UsernameBox;
        TextBox PasswordBox;
        MenuToggler AutoBox;
        MenuToggler SaveBox;

        public override void Init()
        {
            // Prepare menu system
            Menus = new MenuSystem();
            Menus.Init();
            // Create menu items
            MenuButton LoginB = new LoginButton(0, 0);
            PlayOfflineButton PlayOffB = new PlayOfflineButton(0, 0);
            MenuLabel UsernameLabel = new MenuLabel("Username:", 0, 0);
            MenuLabel PasswordLabel = new MenuLabel("Password:", 0, 0);
            UsernameBox = new TextBox(MainGame.ScreenWidth / 2 - 260, 0,
                Texture.GetTexture("menus/textbox_back"), 500, ClientCVar.u_login_username);
            PasswordBox = new TextBox(MainGame.ScreenWidth / 2 - 260, 0,
                Texture.GetTexture("menus/textbox_back"), 500, ClientCVar.u_login_password);
            PasswordBox.Password = true;
            SaveBox = new MenuToggler("Save Username/Password", 0, 0, ClientCVar.u_login_save);
            AutoBox = new MenuToggler("AutoLogin", 0, 0, ClientCVar.u_login_auto);
            // Calculate widths for X-centering
            double lwidth = LoginB.RenderSquare.PositionHigh.X + 10;
            double pwidth = PlayOffB.RenderSquare.PositionHigh.X;
            double ladjust = (MainGame.ScreenWidth / 2 - (lwidth + pwidth) / 2);
            // Adjust their X locations (centering)
            LoginB.RenderSquare.PositionLow.X += ladjust;
            LoginB.RenderSquare.PositionHigh.X += ladjust;
            PlayOffB.RenderSquare.PositionLow.X += ladjust + lwidth;
            PlayOffB.RenderSquare.PositionHigh.X += ladjust + lwidth;
            ladjust = MainGame.ScreenWidth / 2 - LoginB.RenderSquare.PositionHigh.X;
            ladjust = UsernameBox.RenderSquare.PositionLow.X - UsernameLabel.RenderSquare.PositionLow.X;
            UsernameLabel.RenderSquare.PositionLow.X += ladjust;
            UsernameLabel.RenderSquare.PositionHigh.X += ladjust;
            ladjust = PasswordBox.RenderSquare.PositionLow.X - PasswordLabel.RenderSquare.PositionLow.X;
            PasswordLabel.RenderSquare.PositionLow.X += ladjust;
            PasswordLabel.RenderSquare.PositionHigh.X += ladjust;
            ladjust = MainGame.ScreenWidth / 2 - SaveBox.RenderSquare.PositionHigh.X / 2;
            SaveBox.RenderSquare.PositionLow.X += ladjust;
            SaveBox.RenderSquare.PositionHigh.X += ladjust;
            ladjust = MainGame.ScreenWidth / 2 - AutoBox.RenderSquare.PositionHigh.X / 2;
            AutoBox.RenderSquare.PositionLow.X += ladjust;
            AutoBox.RenderSquare.PositionHigh.X += ladjust;
            // Calculate heights for Y-centering
            double uheight = UsernameBox.RenderSquare.PositionHigh.Y + 10;
            double ulheight = UsernameLabel.RenderSquare.PositionHigh.Y + 10;
            double pheight = PasswordBox.RenderSquare.PositionHigh.Y + 10;
            double plheight = PasswordLabel.RenderSquare.PositionHigh.Y + 10;
            double lheight = LoginB.RenderSquare.PositionHigh.Y + 10;
            double sheight = SaveBox.RenderSquare.PositionHigh.Y + 10;
            double aheight = AutoBox.RenderSquare.PositionHigh.Y + 10;
            double theight = uheight + pheight + lheight + ulheight + plheight + sheight + aheight;
            ladjust = MainGame.ScreenHeight / 2 - theight / 2;
            // Adjust their Y position (centering)
            double lat = ladjust;
            UsernameLabel.RenderSquare.PositionLow.Y += lat;
            UsernameLabel.RenderSquare.PositionHigh.Y += lat;
            lat += ulheight;
            UsernameBox.RenderSquare.PositionLow.Y += lat;
            UsernameBox.RenderSquare.PositionHigh.Y += lat;
            lat += uheight;
            PasswordLabel.RenderSquare.PositionLow.Y += lat;
            PasswordLabel.RenderSquare.PositionHigh.Y += lat;
            lat += plheight;
            PasswordBox.RenderSquare.PositionLow.Y += lat;
            PasswordBox.RenderSquare.PositionHigh.Y += lat;
            lat += plheight;
            LoginB.RenderSquare.PositionLow.Y += lat;
            LoginB.RenderSquare.PositionHigh.Y += lat;
            PlayOffB.RenderSquare.PositionLow.Y += lat;
            PlayOffB.RenderSquare.PositionHigh.Y += lat;
            lat += lheight;
            SaveBox.RenderSquare.PositionLow.Y += lat;
            SaveBox.RenderSquare.PositionHigh.Y += lat;
            lat += sheight;
            AutoBox.RenderSquare.PositionLow.Y += lat;
            AutoBox.RenderSquare.PositionHigh.Y += lat;
            // Add items to the menu
            Menus.Add(UsernameLabel);
            Menus.Add(UsernameBox);
            Menus.Add(PasswordLabel);
            Menus.Add(PasswordBox);
            Menus.Add(LoginB);
            Menus.Add(PlayOffB);
            Menus.Add(SaveBox);
            Menus.Add(AutoBox);
            Recalc();
            if (AutoBox.toggled)
            {
                LoginB.LeftClick(0, 0);
            }
            // Done!
            Initted = true;
        }

        public void Recalc()
        {
            // Set the current username/password
            string[] namepass = AccountFileSaver.GetAccountData();
            UsernameBox.TypingText = namepass[0];
            ClientCVar.u_login_username.Set(namepass[0]);
            PasswordBox.TypingText = namepass[1];
            ClientCVar.u_login_password.Set(namepass[1]);
            UsernameBox.FixCursor();
            PasswordBox.FixCursor();
            if (UsernameBox.TypingText.Length > 0)
            {
                SaveBox.toggled = true;
                ClientCVar.u_login_save.Set(true);
            }
            AutoBox.toggled = namepass[2] == "true";
            ClientCVar.u_login_auto.Set(namepass[2] == "true");
        }

        public override void Tick()
        {
            Menus.Tick();
            KeyHandler.GetKBState();
        }

        public override void Draw2D()
        {
            GL.ClearColor(new Color4(0, 200, 200, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Menus.Draw();
        }

        public override void Draw3D()
        {
        }
    }
}

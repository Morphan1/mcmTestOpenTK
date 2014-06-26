using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.UIHandlers.Menus.Login
{
    public class LoginButton: MenuButton
    {
        public LoginButton(int X, int Y)
            : base("Login", X, Y, Texture.GetTexture("menus/button_back_hover"),
            Texture.GetTexture("menus/button_back_standard"))
        {
        }

        public override void LeftClick()
        {
            MainGame.SetScreen(ScreenMode.MainMenu);
        }
    }
}

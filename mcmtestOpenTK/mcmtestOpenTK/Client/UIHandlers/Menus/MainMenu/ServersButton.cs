using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.Networking.OneOffs;

namespace mcmtestOpenTK.Client.UIHandlers.Menus.MainMenu
{
    public class ServersButton: MenuButton
    {
        public ServersButton(int X, int Y)
            : base("Servers", X, Y, Texture.GetTexture("menus/button_back_hover"),
            Texture.GetTexture("menus/button_back_standard"))
        {
        }

        public override void LeftClick(int x, int y)
        {
            MainGame.SetScreen(ScreenMode.Servers);
        }
    }
}

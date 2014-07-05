using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.CommonHandlers;

namespace mcmtestOpenTK.Client.UIHandlers.Menus.Login
{
    public class PlayOfflineButton: MenuButton
    {
        public PlayOfflineButton(int X, int Y)
            : base("Play Offline", X, Y, Texture.GetTexture("menus/button_back_hover"),
            Texture.GetTexture("menus/button_back_standard"))
        {
        }

        public override void LeftClick(int x, int y)
        {
            if (ClientCVar.u_login_username.Value.Length < 6 && ClientCVar.u_login_username.Value.Length < 20)
            {
                Menus.ShowNotice("You must enter a username (it can be anything, as long as it is 6 letters or more)!");
            }
            else
            {
                MainGame.Username = ClientCVar.u_login_username.Value;
                UIConsole.WriteLine("Playing offline as " + ClientCVar.u_login_username.Value);
                MainGame.SetScreen(ScreenMode.MainMenu);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;

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
            Screen_Login loginscr = (Screen_Login)MainGame.Screens[(int)ScreenMode.Login];
            if (loginscr.UsernameBox.TypingText.Length < 6)
            {
                Menus.ShowNotice("You must enter a username (it can be anything, as long as it is 6 letters or more)!");
            }
            else
            {
                MainGame.Username = loginscr.UsernameBox.TypingText;
                UIConsole.WriteLine("Playing offline as " + loginscr.UsernameBox.TypingText);
                MainGame.SetScreen(ScreenMode.MainMenu);
            }
        }
    }
}

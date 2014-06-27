using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.Networking.OneOffs;

namespace mcmtestOpenTK.Client.UIHandlers.Menus.Login
{
    public class LoginButton: MenuButton
    {
        public LoginButton(int X, int Y)
            : base("Login", X, Y, Texture.GetTexture("menus/button_back_hover"),
            Texture.GetTexture("menus/button_back_standard"))
        {
        }

        public override void LeftClick(int x, int y)
        {
            Screen_Login loginscr = (Screen_Login)MainGame.Screens[(int)ScreenMode.Login];
            if (loginscr.UsernameBox.TypingText.Length < 6 || loginscr.PasswordBox.TypingText.Length < 4)
            {
                Menus.ShowNotice("You must enter your username and password!");
            }
            else
            {
                if (loginscr.SaveBox.toggled)
                {
                    AccountFileSaver.SaveAccountData(loginscr.UsernameBox.TypingText, loginscr.PasswordBox.TypingText);
                }
                else
                {
                    AccountFileSaver.SaveAccountData("", "");
                }
                GlobalLoginRequest.RequestLogin(true, loginscr.UsernameBox.TypingText, loginscr.PasswordBox.TypingText);
                Menus.ShowNotice("Logging in...");
            }
        }
    }
}

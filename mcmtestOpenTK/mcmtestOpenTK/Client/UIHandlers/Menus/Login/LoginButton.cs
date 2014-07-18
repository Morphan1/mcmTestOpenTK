using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.Networking.OneOffs;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

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
            if (ClientCVar.u_login_username.Value.Length < 6 || ClientCVar.u_login_password.Value.Length < 4
                || ClientCVar.u_login_username.Value.Length > 20)
            {
                Menus.ShowNotice("You must enter your username and password!");
            }
            else
            {
                if (ClientCVar.u_login_save.ValueB)
                {
                    AccountFileSaver.SaveAccountData(ClientCVar.u_login_username.Value, ClientCVar.u_login_password.Value, ClientCVar.u_login_auto.ValueB ? "true" : "false");
                }
                else
                {
                    AccountFileSaver.SaveAccountData("", "", "");
                }
                GlobalLoginRequest.RequestLogin(true, ClientCVar.u_login_username.Value, ClientCVar.u_login_password.Value);
                Menus.ShowNotice("Logging in...");
            }
        }
    }
}

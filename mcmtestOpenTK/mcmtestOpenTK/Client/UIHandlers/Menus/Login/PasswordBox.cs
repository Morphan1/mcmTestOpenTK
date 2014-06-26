using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.UIHandlers.Menus.Login
{
    public class PasswordBox: TextBox
    {
        public PasswordBox(int X, int Y, Texture _standText, int _width)
            : base(X, Y, _standText, _width)
        {
        }

        public override void Enter()
        {
            ((Screen_Login)MainGame.Screen).LoginB.LeftClick(0, 0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;

namespace mcmtestOpenTK.Client.UIHandlers.Menus.Login
{
    public class LoginBox: TextBox
    {
        public LoginBox(int X, int Y, Texture _standText, int _width)
            : base(X, Y, _standText, _width)
        {
            selected = true;
        }

        public override void Enter()
        {
            Menus.RotateTextBoxSelect();
        }
    }
}

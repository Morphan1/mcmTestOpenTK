using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using mcmtestOpenTK.Client.UIHandlers.Menus;
using mcmtestOpenTK.Client.UIHandlers.Menus.Login;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_Login : AbstractScreen
    {
        MenuSystem Menus;

        public Screen_Login(): base(ScreenMode.Login)
        {
        }

        public override void Init()
        {
            Menus = new MenuSystem();
            Menus.Init();
            Menus.MenuItems.Add(new LoginButton(5, 5));
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
            KeyHandler.Clear();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_MainMenu : AbstractScreen
    {
        public Screen_MainMenu(): base(ScreenMode.MainMenu)
        {
        }

        public override void SwitchTo()
        {
            // TODO: Clicky noises!
        }

        public override void Init()
        {
            Initted = true;
        }

        float blue = 0;

        float bluemod = 1;

        public override void Tick()
        {
            blue += (float)MainGame.Delta * bluemod / 10;
            if (blue >= 1)
            {
                bluemod = -1;
                blue = 1;
            }
            if (blue <= 0)
            {
                bluemod = 1;
                blue = 0;
            }
        }

        public override void Draw2D()
        {
            GL.ClearColor(new Color4(0, 0, (byte)(blue * 255), 255));
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public override void Draw3D()
        {
        }
    }
}

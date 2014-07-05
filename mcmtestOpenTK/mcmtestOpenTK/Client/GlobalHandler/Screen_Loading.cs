using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_Loading : AbstractScreen
    {
        public Screen_Loading(): base(ScreenMode.Loading)
        {
        }

        public override void Init()
        {
            Initted = true;
        }

        public override void SwitchTo()
        {
        }

        public override void SwitchFrom()
        {
        }

        float green = 0;

        float greenmod = 1;

        public override void Tick()
        {
            green += (float)MainGame.Delta * greenmod / 10;
            if (green >= 1)
            {
                greenmod = -1;
                green = 1;
            }
            if (green <= 0)
            {
                greenmod = 1;
                green = 0;
            }
        }

        public override void Draw2D()
        {
            GL.ClearColor(new Color4(0, (byte)(green * 255), 0, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public override void Draw3D()
        {
        }
    }
}

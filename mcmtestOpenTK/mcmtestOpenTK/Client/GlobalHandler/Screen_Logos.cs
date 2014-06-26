using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_Logos : AbstractScreen
    {
        public Screen_Logos(): base(ScreenMode.Logos)
        {
        }

        public override void Init()
        {
            Initted = true;
        }

        public override void SwitchTo()
        {
        }

        double LogoTimer = 1f;

        float red = 0;

        float redmod = 1;

        public override void Tick()
        {
            LogoTimer -= MainGame.Delta;
            red += (float)MainGame.Delta * redmod * 3;
            if (red >= 1)
            {
                redmod = -1;
                red = 1;
            }
            if (red <= 0)
            {
                redmod = 1;
                red = 0;
            }
            if (LogoTimer <= 0)
            {
                MainGame.SetScreen(ScreenMode.Login);
            }
            KeyHandler.GetKBState();
        }

        public override void Draw2D()
        {
            GL.ClearColor(new Color4((byte)(red * 255), 0, 0, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public override void Draw3D()
        {
        }
    }
}

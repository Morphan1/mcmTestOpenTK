using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_Logos : AbstractScreen
    {
        public Screen_Logos(): base(ScreenMode.Logos)
        {
        }

        public override void Init()
        {
            Logo = new Square();
            Logo.PositionLow = new Location(0, 0, 0);
            Logo.PositionHigh = new Location(MainGame.ScreenWidth, MainGame.ScreenHeight, 0);
            Logo.texture = Texture.GetTexture("splashes/logo");
            Logo.shader = Shader.ColorMultShader;
            Initted = true;
        }

        public override void SwitchTo()
        {
        }

        double LogoTimer = 2f;

        float red = 0;

        float redmod = 1;

        public Square Logo;

        public override void Tick()
        {
            LogoTimer -= MainGame.Delta;
            red += (float)MainGame.Delta * redmod;
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
            GL.ClearColor(red, 1f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Color4(red, 1f, 1f, 1f);
            Logo.Draw();
        }

        public override void Draw3D()
        {
        }
    }
}

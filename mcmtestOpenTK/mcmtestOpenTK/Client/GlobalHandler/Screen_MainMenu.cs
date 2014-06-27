using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using mcmtestOpenTK.Client.UIHandlers.Menus;
using mcmtestOpenTK.Client.UIHandlers.Menus.MainMenu;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_MainMenu : AbstractScreen
    {
        MenuSystem Menus;

        public Screen_MainMenu(): base(ScreenMode.MainMenu)
        {
        }

        public override void SwitchTo()
        {
            // TODO: Clicky noises!
        }

        public override void Init()
        {
            // Create system
            Menus = new MenuSystem();
            Menus.Init();
            // Create items
            ReloginButton relogbutton = new ReloginButton(0, MainGame.ScreenHeight / 2);
            ServersButton sbutton = new ServersButton(0, MainGame.ScreenHeight / 2);
            // Adjust X
            sbutton.RenderSquare.PositionLow.X += relogbutton.RenderSquare.PositionHigh.X + 10;
            sbutton.RenderSquare.PositionHigh.X += relogbutton.RenderSquare.PositionHigh.X + 10;
            relogbutton.RenderSquare.PositionLow.X += 5;
            relogbutton.RenderSquare.PositionHigh.X += 5;
            // Add items
            Menus.Add(relogbutton);
            Menus.Add(sbutton);
            // Done!
            Initted = true;
        }

        public override void Tick()
        {
            Menus.Tick();
        }

        public override void Draw2D()
        {
            GL.ClearColor(new Color4(0, 100, 255, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Menus.Draw();
        }

        public override void Draw3D()
        {
        }
    }
}

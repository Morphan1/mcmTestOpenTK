using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using mcmtestOpenTK.Client.UIHandlers.Menus;
using mcmtestOpenTK.Client.UIHandlers.Menus.MainMenu;
using mcmtestOpenTK.Client.Networking.OneOffs;
using System.Drawing;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    class Screen_Servers : AbstractScreen
    {
        public static List<PingedServer> ServerList;

        public static void AddServer(PingedServer serv)
        {
            for (int i = 0; i < ServerList.Count; i++)
            {
                if (ServerList[i].Address == serv.Address && ServerList[i].Port == serv.Port)
                {
                    ServerList[i] = serv;
                    return;
                }
            }
            ServerList.Add(serv);
        }

        public Screen_Servers(): base(ScreenMode.MainMenu)
        {
            ServerList = new List<PingedServer>();
        }

        public override void SwitchTo()
        {
        }

        public override void Init()
        {
            Initted = true;
        }

        public override void Tick()
        {
        }

        public override void Draw2D()
        {
            GL.ClearColor(new Color4(0, 100, 255, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit);
            int Y = 10;
            int XMin = 10;
            int XMax = MainGame.ScreenWidth - 10;
            for (int i = 0; i < ServerList.Count; i++)
            {
                Square.DrawColoredSquare(XMin, Y, XMax, (int)(Y + GLFont.Standard.Height * 2), Color.DarkBlue);
                FontSet.Standard.DrawColoredText("Server: " + ServerList[i].Name + TextStyle.Default + ", ping: " + ServerList[i].Ping
                    + "\nAddress: " + ServerList[i].Address + TextStyle.Default + ":" + ServerList[i].Port, new Location(XMin + 5, Y, 0));
                Y += (int)(GLFont.Standard.Height * 2 + 10);
            }
        }

        public override void Draw3D()
        {
        }
    }
}

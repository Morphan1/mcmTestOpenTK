using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using mcmtestOpenTK.Client.CommonHandlers;
using System.IO;
using System.Diagnostics;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        /// <summary>
        /// Global entry point, should never be directly called!
        /// </summary>
        /// <param name="args">Command line input args</param>
        public static void Client_Main(List<string> args)
        {
            // Fix the system console.
            SysConsole.FixTitle();
            // Create the window and establish basic event info / settings
            PrimaryGameWindow = new GameWindow(ScreenWidth, ScreenHeight);
            PrimaryGameWindow.Title = WindowTitle;
            PrimaryGameWindow.Closed += new EventHandler<EventArgs>(PrimaryGameWindow_Closed);
            PrimaryGameWindow.Load += new EventHandler<EventArgs>(PrimaryGameWindow_Load);
            PrimaryGameWindow.UpdateFrame += new EventHandler<FrameEventArgs>(PrimaryGameWindow_UpdateFrame);
            PrimaryGameWindow.RenderFrame += new EventHandler<FrameEventArgs>(PrimaryGameWindow_RenderFrame);
            PrimaryGameWindow.KeyPress += new EventHandler<KeyPressEventArgs>(KeyHandler.PrimaryGameWindow_KeyPress);
            PrimaryGameWindow.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyHandler.PrimaryGameWindow_KeyDown);
            PrimaryGameWindow.KeyUp += new EventHandler<KeyboardKeyEventArgs>(KeyHandler.PrimaryGameWindow_KeyUp);
            // Begin running the game.
            SysConsole.Output(OutputType.INIT, "Starting up main game window...");
            PrimaryGameWindow.Run(Target_cFPS, Target_gFPS);
            SysConsole.Output(OutputType.CLIENTINFO, "Game done running!");
        }
    }
}

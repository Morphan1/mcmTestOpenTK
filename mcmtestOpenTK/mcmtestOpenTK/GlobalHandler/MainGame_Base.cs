﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.CommonHandlers;

namespace mcmtestOpenTK.GlobalHandler
{
    public partial class MainGame
    {
        /// <summary>
        /// Global entry point, should never be directly called!
        /// </summary>
        /// <param name="args">Command line input args</param>
        [STAThread]
        static void Main(string[] args)
        {
            // Utilties are prepared before anything else
            Util.Init();
            // Create the window and establish basic event info / settings
            PrimaryGameWindow = new GameWindow(ScreenWidth, ScreenHeight);
            PrimaryGameWindow.Title = WindowTitle;
            PrimaryGameWindow.Load += new EventHandler<EventArgs>(PrimaryGameWindow_Load);
            PrimaryGameWindow.UpdateFrame += new EventHandler<FrameEventArgs>(PrimaryGameWindow_UpdateFrame);
            PrimaryGameWindow.RenderFrame += new EventHandler<FrameEventArgs>(PrimaryGameWindow_RenderFrame);
            PrimaryGameWindow.KeyPress += new EventHandler<KeyPressEventArgs>(PrimaryGameWindow_KeyPress);
            // Begin running the game.
            PrimaryGameWindow.Run(Target_cFPS, Target_gFPS);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.GraphicsHandlers;

namespace mcmtestOpenTK.GlobalHandler
{
    public partial class MainGame
    {
        // Globally required
        public static GameWindow PrimaryGameWindow;
        // Basic settings
        public static int ScreenWidth = 1500;
        public static int ScreenHeight = 600;
        public static int VSync = 0;
        public static int Target_cFPS = 60;
        public static int Target_gFPS = 60;
        public static int FontSize = 12;
        public static string WindowTitle = "mcmonkey's OpenTK Test Game";
        // Public data on current tick
        public static double Delta = 1;
        public static int cFPS = 0;
        public static MouseState CurrentMouse;
        public static KeyboardState CurrentKeyboard;
        public static MouseState PreviousMouse;
        public static KeyboardState PreviousKeyboard;
        // Public data on current render tick
        public static double GraphicsDelta = 1;
        public static int gFPS = 0;
        public static bool IsFirstGraphicsDraw = true;

        // Temporary, for testing
        public static int X = 0;
        public static int Y = 0;
        public static PieceOfText debug;
        public static PieceOfText input;
    }
}

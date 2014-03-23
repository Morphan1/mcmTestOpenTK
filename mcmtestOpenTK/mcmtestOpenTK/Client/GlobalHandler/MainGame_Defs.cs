using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        // Globally required
        public static GameWindow PrimaryGameWindow;

        // Basic settings
        public static int ScreenWidth = 1000;
        public static int ScreenHeight = 600;
        public static int Target_cFPS = 60;
        public static int Target_gFPS = 60;
        public static int FontSize = 12;
        public static double MouseSensitivity = 3f;
        public static string WindowTitle = "mcmonkey's OpenTK Test Game";

        // Public data on current CPU processing tick
        /// <summary>
        /// How many seconds have passed this tick.
        /// </summary>
        public static double Delta = 0;
        /// <summary>
        /// How many seconds have passed this tick.
        /// </summary>
        public static float DeltaF = 0;
        /// <summary>
        /// The current CPU processing Frames Per Second.
        /// </summary>
        public static int cFPS = 0;

        // Public data on current render tick
        /// <summary>
        /// How many seconds have passed this graphics tick.
        /// </summary>
        public static double GraphicsDelta = 0;
        /// <summary>
        /// How many seconds have passed this graphics tick.
        /// </summary>
        public static float GraphicsDeltaF = 0;
        /// <summary>
        /// The current GPU processing Frames Per Second.
        /// </summary>
        public static int gFPS = 0;
        public static Vector3 Forward = Vector3.UnitX;
        public static bool IsFirstGraphicsDraw = true;

        /// <summary>
        /// Whether the system is currently loading.
        /// </summary>
        public static bool Initializing = true;

        // Temporary, for testing
        public static int X = 0;
        public static int Y = 0;
        public static PieceOfText debug;

        // Public gameplay data
        public static List<Entity> entities = new List<Entity>();
    }
}

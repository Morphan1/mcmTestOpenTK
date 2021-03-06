﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        // Globally required

        /// <summary>
        /// The primary client game window.
        /// </summary>
        public static GameWindow PrimaryGameWindow;

        // Basic settings

        /// <summary>
        /// How wide the screen is, in pixels.
        /// </summary>
        public static int ScreenWidth = 800;

        /// <summary>
        /// How tall the screen is, in pixels.
        /// </summary>
        public static int ScreenHeight = 600;

        /// <summary>
        /// The target computational frames per second.
        /// TODO: HANDLE IN A CVAR
        /// </summary>
        public static int Target_cFPS = 60;

        /// <summary>
        /// The target graphical frames per second.
        /// TODO: HANDLE IN A CVAR
        /// </summary>
        public static int Target_gFPS = 60;

        /// <summary>
        /// Default font size.
        /// TODO: HANDLE IN A CVAR
        /// </summary>
        public static int FontSize = 10;
        
        /// <summary>
        /// How sensitive the mouse is.
        /// TODO: HANDLE IN A CVAR
        /// </summary>
        public static double MouseSensitivity = 3f;

        /// <summary>
        /// The title of the client window.
        /// </summary>
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

        /// <summary>
        /// The forward vector of the player's view.
        /// </summary>
        public static Location Forward = Location.UnitX;

        /// <summary>
        /// Whether this tick is the first time graphics has been called since reload.
        /// </summary>
        public static bool IsFirstGraphicsDraw = true;

        /// <summary>
        /// Whether the system is currently rendering in wireframe-mode.
        /// </summary>
        public static bool IsWireFrame = false;

        /// <summary>
        /// Whether the system is currently loading.
        /// </summary>
        public static bool Initializing = true;

        /// <summary>
        /// Whether the client has been spawned into a world.
        /// </summary>
        public static bool Spawned = false;

        // Account Data

        /// <summary>
        /// The logged-in account username.
        /// </summary>
        public static string Username = "";

        /// <summary>
        /// The logged-in account password.
        /// </summary>
        public static string Password = "";

        /// <summary>
        /// The logged-in account session key.
        /// </summary>
        public static string Session = "";

        /// <summary>
        /// The current global time.
        /// </summary>
        public static double GlobalTickTime;

        /// <summary>
        /// What screen the game is currently on.
        /// </summary>
        public static AbstractScreen Screen;

        /// <summary>
        /// All loaded screens.
        /// </summary>
        public static AbstractScreen[] Screens;

        /// <summary>
        /// What face is currently being culled.
        /// </summary>
        public static CullFaceMode CullFace;

        /// <summary>
        /// The general shader models should use if they lack their own.
        /// </summary>
        public static Shader GeneralShader;

        /// <summary>
        /// Whether to take a screenshot.
        /// </summary>
        public static bool Screenshot;

        static Queue<Bitmap> Screenshots = new Queue<Bitmap>();

        static Object ScreenshotLock = new Object();

        static volatile string ConfigStr = "";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GameplayHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Client.Networking.OneOffs;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        /// <summary>
        /// Called when the game window is loaded up, should initialize everything.
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Irrelevant</param>
        static void PrimaryGameWindow_Load(object sender, EventArgs e)
        {
            try
            {
                // Always at the start
                Initializing = true;
                GlobalTickTime = 0;
                ClientOutputter cout = new ClientOutputter();
                cout.Initializing = true;
                // Build screen list
                Screens = new AbstractScreen[(int)ScreenMode.MAX];
                Screens[(int)ScreenMode.Logos] = new Screen_Logos();
                Screens[(int)ScreenMode.Login] = new Screen_Login();
                Screens[(int)ScreenMode.MainMenu] = new Screen_MainMenu();
                Screens[(int)ScreenMode.Loading] = new Screen_Loading();
                Screens[(int)ScreenMode.Game] = new Screen_Game();
                // Prepare the CVar system
                SysConsole.Output(OutputType.INIT, "Preparing CVars...");
                ClientCVar.Init(cout);
                // Prepares the command system
                SysConsole.Output(OutputType.INIT, "Preparing command system...");
                ClientCommands.Init(cout);
                // Prepare key system
                SysConsole.Output(OutputType.INIT, "Preparing key bind system...");
                KeyHandler.Init();
                // TODO: Load default.cfg / etc.
                // Prepare text output / language related info
                SysConsole.Output(OutputType.INIT, "Preparing text/languaging...");
                LanguageHandler.Init();
                // Handle some internal CVar-based graphics settings
                SysConsole.Output(OutputType.INIT, "Preparing general graphics...");
                ReloadGraphics();
                // Prepare the shader system
                SysConsole.Output(OutputType.INIT, "Preparing shaders...");
                Shader.InitShaderSystem();
                // Prepare the texture system
                SysConsole.Output(OutputType.INIT, "Preparing textures...");
                Texture.InitTextureSystem();
                // Load text font data
                SysConsole.Output(OutputType.INIT, "Preparing text...");
                GLFont.Init();
                FontSet.Init();
                // Set the title
                SysConsole.Output(OutputType.INIT, "Preparing window settings...");
                PrimaryGameWindow.Title = WindowTitle;
                // Disable user-induced window resizing. Only do this from the code.
                PrimaryGameWindow.WindowBorder = WindowBorder.Fixed;
                // Set the background color to clear to
                GL.ClearColor(Color.Black);
                // Textures are always enabled and in-use
                GL.Enable(EnableCap.Texture2D);
                // Prepare the console
                SysConsole.Output(OutputType.INIT, "Preparing game console...");
                UIConsole.InitConsole();
                // Prepare audio-related code
                SysConsole.Output(OutputType.INIT, "Preparing sound system...");
                Sound.InitSoundSystem();
                // Prepare networking code
                SysConsole.Output(OutputType.INIT, "Preparing networking...");
                NetPing.Init();
                // Prepare gameplay-related code
                SysConsole.Output(OutputType.INIT, "Preparing gameplay system...");
                Player.player = new Player();
                LoadWorld();
                SysConsole.Output(OutputType.INIT, "Preparing start screen...");
                SetScreen(ScreenMode.Logos);
                // Everything's loaded now... scrap the system console
                SysConsole.Output(OutputType.INIT, "System prepared, hiding console and playing the game!");
                SysConsole.HideConsole();
                // Always at the end
                Initializing = false;
                cout.Initializing = false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("MainGame/Load", ex);
            }
        }

        /// <summary>
        /// Call to recalculate graphics related code.
        /// </summary>
        public static void ReloadGraphics()
        {
            // Tell everything to re-calculate with disregard for modified state.
            IsFirstGraphicsDraw = true;
            // Setup VSync mode
            SysConsole.Output(OutputType.INIT, "Setting VSYNC to " + TextStyle.Color_Separate + ClientCVar.r_vsync.ValueI);
            switch (ClientCVar.r_vsync.ValueI)
            {
                case 0:
                    PrimaryGameWindow.VSync = VSyncMode.Off;
                    break;
                case 1:
                    PrimaryGameWindow.VSync = VSyncMode.On;
                    break;
                default:
                    PrimaryGameWindow.VSync = VSyncMode.Adaptive;
                    break;
            }
            // Update the screen size
            if (ScreenWidth != ClientCVar.r_screenwidth.ValueI || ScreenHeight != ClientCVar.r_screenheight.ValueI)
            {
                SysConsole.Output(OutputType.INIT, "Setting SCREEN SIZE to " + TextStyle.Color_Separate + ScreenWidth + ", " + ScreenHeight);
                if (ClientCVar.r_screenwidth.ValueI < 300)
                {
                    ClientCVar.r_screenwidth.Set("300");
                }
                if (ClientCVar.r_screenheight.ValueI < 300)
                {
                    ClientCVar.r_screenheight.Set("300");
                }
                int XAdjust = ClientCVar.r_screenwidth.ValueI - ScreenWidth;
                int YAdjust = ClientCVar.r_screenheight.ValueI - ScreenHeight;
                ScreenWidth = ClientCVar.r_screenwidth.ValueI;
                ScreenHeight = ClientCVar.r_screenheight.ValueI;
                PrimaryGameWindow.Size = new Size(ScreenWidth, ScreenHeight);
                UIConsole.Typing.Position.Y += YAdjust / 2;
                UIConsole.ScrollText.Position.Y += YAdjust / 2;
                UIConsole.MaxWidth += XAdjust;
            }
            // Handle fullscreen state
            if (ClientCVar.r_fullscreen.ValueB)
            {
                SysConsole.Output(OutputType.INIT, "Enabling FULLSCREEN");
                PrimaryGameWindow.WindowState = WindowState.Fullscreen;
            }
            else
            {
                SysConsole.Output(OutputType.INIT, "Disabling FULLSCREEN");
                PrimaryGameWindow.WindowState = WindowState.Normal;
            }
            // Correct the viewport (screen size, fullscreen, etc. might affect)
            GL.Viewport(0, 0, ScreenWidth, ScreenHeight);
        }

        public static void SetScreen(ScreenMode mode)
        {
            AbstractScreen screen = Screens[(int)mode];
            if (!screen.Initted)
            {
                SysConsole.Output(OutputType.INIT, "Prepare screen " + mode);
                screen.Init();
            }
            screen.SwitchTo();
            Screen = screen;
        }
    }
}

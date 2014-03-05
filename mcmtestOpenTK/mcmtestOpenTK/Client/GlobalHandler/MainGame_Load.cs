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
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Client.Networking;

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
                // Prepare graphics-related code
                ReloadGraphics();
                debug = new PieceOfText("", new Point(5, ScreenHeight / 5 * 3));
                // Prepare the console
                UIConsole.InitConsole();
                // Prepares the command system
                Commands.Init();
                // Prepare audio-related code
                SimpleAudioTest.LoadSound();
                // Prepare networking code
                GlobalNetwork.Init();
                // Prepare gameplay-related code
                Player.player = new Player();
                SpawnEntity(Player.player);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }

        /// <summary>
        /// Called when the window is changed, to handle graphics changes.
        /// </summary>
        static void ReloadGraphics()
        {
            // Prepare the shader system
            Shader.InitShaderSystem();
            // Tell everything to re-calculate with disregard for modified state.
            IsFirstGraphicsDraw = true;
            // Prepare the texture system
            Texture.InitTextureSystem();
            // Load text font data
            GLFont.Init();
            // Set the title
            PrimaryGameWindow.Title = WindowTitle;
            // Setup VSync mode
            switch (VSync)
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
            // Disable user-induced window resizing. Only do this from the code.
            PrimaryGameWindow.WindowBorder = WindowBorder.Fixed;
            // Set the background color to clear to
            GL.ClearColor(Color.Black);
            // Re-create the "Perspective" matrix
            float FOVradians = MathHelper.DegreesToRadians(45);
            Perspective = Matrix4.CreatePerspectiveFieldOfView(FOVradians, (float)ScreenWidth / (float)ScreenHeight, 1, 4000);
            // Textures are always enabled and in-use
            GL.Enable(EnableCap.Texture2D);
        }
    }
}

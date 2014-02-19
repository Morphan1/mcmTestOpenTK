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
                TextRenderer.Init();
                /*TextRenderer.Primary.texts.Add(new PieceOfText("ABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz\n1234567890\nXX XX\tXX\nXX\tXX\nX\tXX\nX\tX\n" +
                                                               "TESTCOLORS^1RED^2GREEN^3THREE\n" +
                                                               "^r^7Text Colors: ^0^h^1^^n1 ^!^^n! ^2^^n2 ^@^^n@ ^3^^n3 ^#^^n# ^4^^n4 ^$^^n$ ^5^^n5 ^%^^n% ^6^^n6 ^-^^n- ^7^^n7 ^&^^n& ^8^^n8 ^*^^** ^9^^n9 ^(^^n( ^&^h^0^^n0^h ^)^^n) ^a^^na ^A^^nA\n" +
                                            "^7Text styles: ^b^^nb is bold,^r ^i^^ni is italic,^r ^u^^nu is underline,^r ^s^^ns is strike-through,^r ^O^^nO is overline,^r ^7^h^0^^nh is highlight,^r^7 ^j^^nj is jello (AKA jiggle), ^r\n" +
                                            "^2^e^0^^ne is emphasis,^r^7 ^t^^nt is transparent,^r ^T^^nT is more transparent,^r ^o^^no is opaque,^r ^R^^nR is random,^r ^p^^np is pseudo-random,^r ^^nk is obfuscated (^kobfu^r),^r\n" +
                                            "^^nS is ^SSuperScript^r, ^^nl is ^lSubScript (AKA Lower-Text)^r, ^h^8^d^^nd is Drop-Shadow,^r^7 ^f^^nf is flip,^r ^^nr is regular text, ^^nq is a ^qquote^q, and ^^nn is nothing (escape-symbol).", new Point(10, 10)));*/
                debug = new PieceOfText("", new Point(5, ScreenHeight / 5 * 3));
                TextRenderer.Primary.AddText(debug);
                // Prepare the console
                UIConsole.InitConsole();
                // Prepares the command system
                Commands.Init();
                // Prepare audio-related code
                SimpleAudioTest.LoadSound();
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
            FontHandler.Init();
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

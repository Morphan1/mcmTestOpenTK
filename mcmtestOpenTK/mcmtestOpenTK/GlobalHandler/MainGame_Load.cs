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
using mcmtestOpenTK.GraphicsHandlers.Text;
using mcmtestOpenTK.AudioHandlers;
using mcmtestOpenTK.CommonHandlers;
using mcmtestOpenTK.GameplayHandlers.Entities;
using mcmtestOpenTK.UIHandlers;

namespace mcmtestOpenTK.GlobalHandler
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
                TextRenderer.Primary.texts.Add(new PieceOfText("ABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz\n1234567890\nXX XX\tXX\nXX\tXX\nX\tXX\nX\tX\n" +
                                                               "TESTCOLORS^1RED^2GREEN^3THREE\n" +
                                                               "^r^7Text Colors: ^0^h^1^^11 ^!^^!! ^2^^22 ^@^^@@ ^3^^33 ^#^^## ^4^^44 ^$^^$$ ^5^^55 ^%^^%% ^6^^66 ^-^^-- ^7^^77 ^&^^&& ^8^^88 ^*^^** ^9^^99 ^(^^(( ^&^h^0^^00^h ^)^^)) ^a^^aa ^A^^AA\n" +
                                            "^7Text styles: ^b^^7b is bold,^r ^i^^7i is italic,^r ^u^^7u is underline,^r ^s^^7s is strike-through,^r ^O^^7O is overline,^r ^7^h^0^^0h is highlight,^r^7 ^j^^jj is jello (AKA jiggle), ^r\n" +
                                            " ^2^e^0^^0e is emphasis,^r^7 ^t^^7t is transparent,^r ^T^^7T is more transparent,^r ^o^^7o is opaque,^r ^R^^RR is random,^r ^p^^pp is pseudo-random,^r ^^7k is obfuscated (^kobfu^r),^r\n" +
                                            " ^^7S is ^SSuperScript^r, ^^7l is ^lSubScript (AKA Lower-Text)^r, ^h^8^d^^8d is Drop-Shadow,^r^7 ^f^^7f is flip,^r ^^7r is regular text.", new Point(10, 10)));
                debug = new PieceOfText("", new Point(5, ScreenHeight / 5 * 3));
                TextRenderer.Primary.texts.Add(debug);
                input = new PieceOfText("", new Point(ScreenWidth / 3, 10));
                TextRenderer.Primary.texts.Add(input);
                // Prepare the console
                UIConsole.InitConsole();
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
            // Tell everything to re-calculate with disregard for modified state.
            IsFirstGraphicsDraw = true;
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
        }
    }
}

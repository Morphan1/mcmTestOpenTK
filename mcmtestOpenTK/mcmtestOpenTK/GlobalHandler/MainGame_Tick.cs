using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.CommonHandlers;
using mcmtestOpenTK.AudioHandlers;

namespace mcmtestOpenTK.GlobalHandler
{
    public partial class MainGame
    {
        static int cticknumber = 0;
        static double ctickdelta = 0;
        static int keymark_bouncer = 0;
        static bool keymark_add = false;
        static string rendertext = "";
        static float movetestX;
        static float movetestY;
        /// <summary>
        /// Called every update tick - should handle all logic!
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the delta timing information</param>
        static void PrimaryGameWindow_UpdateFrame(object sender, FrameEventArgs e)
        {
            try
            {
                // Record delta: always first!
                Delta = e.Time;
                // Calculate cFPS: always first!
                cticknumber++;
                ctickdelta += Delta;
                if (ctickdelta > 1.0f)
                {
                    cFPS = cticknumber;
                    cticknumber = 0;
                    ctickdelta = 0.0f;
                }

                // Record current input
                PreviousMouse = CurrentMouse;
                PreviousKeyboard = CurrentKeyboard;
                CurrentMouse = Mouse.GetState();
                CurrentKeyboard = Keyboard.GetState();
                // Prevent null errors in very first tick.
                if (PreviousMouse == null)
                {
                    PreviousMouse = CurrentMouse;
                    PreviousKeyboard = CurrentKeyboard;
                }

                // Update audio
                SimpleAudioTest.RecalculateChannels();

                // Update the input line
                keymark_bouncer++;
                if (keymark_bouncer == 30)
                {
                    keymark_add = !keymark_add;
                    keymark_bouncer = 0;
                }
                if (KeyboardString_InitBS > 0)
                {
                    if (rendertext.Length > KeyboardString_InitBS)
                    {
                        rendertext = rendertext.Substring(0, rendertext.Length - KeyboardString_InitBS);
                    }
                    else
                    {
                        rendertext = "";
                    }
                    KeyboardString_InitBS = 0;
                }
                if (KeyboardString.Length > 0)
                {
                    rendertext += KeyboardString;
                    KeyboardString = "";
                }
                input.Text = rendertext + (keymark_add ? "|" : "");
                if (KeyboardString_CopyPressed)
                {
                    System.Windows.Forms.Clipboard.SetText(rendertext, System.Windows.Forms.TextDataFormat.Text);
                    KeyboardString_CopyPressed = false;
                }

                // Temporary for testing
                X = PrimaryGameWindow.Mouse.X;
                Y = PrimaryGameWindow.Mouse.Y;
                if (CurrentKeyboard.IsKeyDown(Key.Left))
                {
                    movetestX -= (float)(Delta * 100f);
                }
                else if (CurrentKeyboard.IsKeyDown(Key.Right))
                {
                    movetestX += (float)(Delta * 100f);
                }
                if (CurrentKeyboard.IsKeyDown(Key.Up))
                {
                    movetestY -= (float)(Delta * 100f);
                }
                else if (CurrentKeyboard.IsKeyDown(Key.Down))
                {
                    movetestY += (float)(Delta * 100f);
                }
                if (CurrentKeyboard.IsKeyDown(Key.Space) && !PreviousKeyboard.IsKeyDown(Key.Space))
                {
                    Console.WriteLine("Playing audio!");
                    SimpleAudioTest.PlayTestSound();
                }
                debug.Text = "Delta: " + (Delta * 1000) + "\nGraphics Delta: " + (GraphicsDelta * 1000) + "\ncFPS: " + cFPS + "\ngFPS: " + gFPS;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }
    }
}

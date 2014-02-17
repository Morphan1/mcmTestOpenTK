using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        public static int cticknumber = 0;
        static double ctickdelta = 0;
        static float movetestX;
        static float movetestY;
        /// <summary>
        /// Called every update tick - should handle all logic!
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Holds the delta timing information</param>
        static void PrimaryGameWindow_UpdateFrame(object sender, FrameEventArgs EventArgs)
        {
            try
            {
                // Record delta: always first!
                Delta = EventArgs.Time;
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
                PreviousKeyboard = CurrentKeyboard;
                CurrentKeyboard = Keyboard.GetState();
                MouseHandler.Tick();

                // Closeable yay
                if (CurrentKeyboard.IsKeyDown(Key.Escape))
                {
                    Exit();
                }

                // Update console
                UIConsole.Tick();

                // Update audio
                SimpleAudioTest.RecalculateChannels();

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
                    //SimpleAudioTest.PlayTestSound();
                }

                // Update all entities
                for (int i = 0; i < entities.Count; i++)
                {
                    Entity e = entities[i];
                    e.Update();
                }

                // Debug stuff, always near end
                debug.Text = TextStyle.Color_Readable +
                    "Delta: " + ((float)Delta) +
                    "\nGraphics Delta: " + ((float)GraphicsDelta) +
                    "\ncFPS: " + cFPS +
                    "\ngFPS: " + gFPS +
                    "\nPos: " + Player.player.Location.ToString() +
                    "\nAngle: " + Player.player.Angle.ToString() +
                    "\nNow: " + Utilities.DateTimeToString(DateTime.Now) +
                    "\nConsole: " + (UIConsole.Open ? "Open ": "Closed ") + UIConsole.ConsoleText.Position.Y + "... typing: " + UIConsole.Typing.Position.Y;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }
    }
}

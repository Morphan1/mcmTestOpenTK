﻿using System;
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
using mcmtestOpenTK.Client.Networking.Global;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommandHandlers;

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
                DeltaF = (float)Delta;
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
                MouseHandler.Tick();
                KeyHandler.Tick();

                // Closeable yay
                if (KeyHandler.CurrentKeyboard.IsKeyDown(Key.Escape))
                {
                    Exit();
                }

                // Update console
                UIConsole.Tick();

                // Update audio
                SimpleAudioTest.RecalculateChannels();

                // Update networking
                GlobalNetwork.Tick();

                // Update running commands
                Commands.Tick();

                // Update gameplay
                X = PrimaryGameWindow.Mouse.X;
                Y = PrimaryGameWindow.Mouse.Y;
                if (KeyHandler.IsDown(Key.Left))
                {
                    movetestX -= (float)(Delta * 100f);
                }
                else if (KeyHandler.IsDown(Key.Right))
                {
                    movetestX += (float)(Delta * 100f);
                }
                if (KeyHandler.IsDown(Key.Up))
                {
                    movetestY -= (float)(Delta * 100f);
                }
                else if (KeyHandler.IsDown(Key.Down))
                {
                    movetestY += (float)(Delta * 100f);
                }
                if (KeyHandler.IsPressed(Key.Space))
                {
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
                    "\ncFPS: " + cFPS +
                    "\ngFPS: " + gFPS +
                    "\nPosition: " + Player.player.Location.ToString() +
                    "\nAngle: " + Player.player.Angle.ToString() +
                    "\nNow: " + Utilities.DateTimeToString(DateTime.Now);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }
    }
}

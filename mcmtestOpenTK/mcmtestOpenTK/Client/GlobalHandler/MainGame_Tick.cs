﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GameplayHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking.Global;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Client.Networking;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        public static int cticknumber = 0;
        static double ctickdelta = 0;
        public static float pingbump = 0;

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
                if (ctickdelta >= 1.0f)
                {
                    cFPS = cticknumber;
                    cticknumber = 0;
                    ctickdelta -= 1.0f;
                }
                GlobalTickTime += (int)(Delta * 1000);

                // Calculate ping
                pingbump += DeltaF;

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
                Sound.RecalculateChannels();

                // Update networking
                NetworkBase.Tick();
                GlobalNetwork.Tick();

                // Update running commands
                ClientCommands.Tick();

                // Update gameplay
                X = PrimaryGameWindow.Mouse.X;
                Y = PrimaryGameWindow.Mouse.Y;

                // Update player
                Player.player.Update();

                // Update world
                TickWorld();

                // Debug stuff, always near end
                debug.Text = TextStyle.Color_Readable +
                    "\ncFPS: " + cFPS +
                    "\ngFPS: " + gFPS +
                    "\nPosition: " + Player.player.Position.ToString() +
                    "\nDirection: " + Player.player.Direction.ToString() +
                    "\nVelocity: " + Player.player.Velocity.ToString() +
                    "\nNow: " + Utilities.DateTimeToString(DateTime.Now) +
                    "\nPing: " + (int)(Ping * 1000);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("MainGame/Tick", ex);
            }
        }
    }
}

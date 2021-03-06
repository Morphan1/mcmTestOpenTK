﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.Util;

namespace mcmtestOpenTK.Client.UIHandlers
{
    public class MouseHandler
    {
        /// <summary>
        /// Whether the mouse is captured.
        /// </summary>
        public static bool MouseCaptured = false;

        /// <summary>
        /// How much the mouse has moved this tick.
        /// </summary>
        public static Location MouseDelta = new Location();

        /// <summary>
        /// The current mouse state for this tick.
        /// </summary>
        public static MouseState CurrentMouse;

        /// <summary>
        /// The mouse state during the previous tick.
        /// </summary>
        public static MouseState PreviousMouse;

        /// <summary>
        /// How much the mouse was scrolled this tick.
        /// </summary>
        public static int MouseScroll = 0;

        /// <summary>
        /// Captures the mouse to this window.
        /// </summary>
        public static void CaptureMouse()
        {
            CenterMouse();
            MouseCaptured = true;
            MainGame.PrimaryGameWindow.CursorVisible = false;
        }

        /// <summary>
        /// Uncaptures the mouse for this window.
        /// </summary>
        public static void ReleaseMouse()
        {
            MouseCaptured = false;
            MainGame.PrimaryGameWindow.CursorVisible = true;
        }

        /// <summary>
        /// Moves the mouse back to the center position.
        /// </summary>
        public static void CenterMouse()
        {
            Point center = MainGame.PrimaryGameWindow.PointToScreen(new Point(MainGame.ScreenWidth / 2, MainGame.ScreenHeight / 2));
            Mouse.SetPosition(center.X, center.Y);
        }

        public static float pwheelstate;
        public static float cwheelstate;

        public static int MouseX()
        {
            return MainGame.PrimaryGameWindow.Mouse.X;
        }

        public static int MouseY()
        {
            return MainGame.PrimaryGameWindow.Mouse.Y;
        }

        /// <summary>
        /// Updates mouse movement.
        /// </summary>
        public static void Tick()
        {
            if (MainGame.PrimaryGameWindow.Focused && MouseCaptured)
            {
                double MoveX = (((MainGame.ScreenWidth / 2) - MouseX()) * MainGame.Delta * MainGame.MouseSensitivity);
                double MoveY = (((MainGame.ScreenHeight / 2) - MouseY()) * MainGame.Delta * MainGame.MouseSensitivity);
                MouseDelta = new Location((float)MoveX, (float)MoveY, 0);
                CenterMouse();
                PreviousMouse = CurrentMouse;
                CurrentMouse = Mouse.GetState();
                pwheelstate = cwheelstate;
                cwheelstate = CurrentMouse.WheelPrecise;
                MouseScroll = (int)(cwheelstate - pwheelstate);
            }
            else
            {
                MouseDelta = Location.Zero;
            }
            if (MainGame.PrimaryGameWindow.Focused && !MouseCaptured)
            {
                PreviousMouse = CurrentMouse;
                CurrentMouse = Mouse.GetState();
                pwheelstate = cwheelstate;
                cwheelstate = CurrentMouse.WheelPrecise;
                MouseScroll = (int)(cwheelstate - pwheelstate);
            }
            if (!MainGame.PrimaryGameWindow.Focused)
            {
                cwheelstate = Mouse.GetState().WheelPrecise;
                pwheelstate = cwheelstate;
            }
        }
    }
}

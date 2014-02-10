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
using mcmtestOpenTK.CommonHandlers;

namespace mcmtestOpenTK.GlobalHandler
{
    public partial class MainGame
    {
        static int gticknumber = 0;
        static double gtickdelta = 0;
        /// <summary>
        /// Called every render tick - should handle all graphics!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void PrimaryGameWindow_RenderFrame(object sender, FrameEventArgs e)
        {
            try
            {
                // Record delta: always first!
                GraphicsDelta = e.Time;
                // Calculate gFPS: always first!
                gticknumber++;
                gtickdelta += GraphicsDelta;
                if (gtickdelta > 1.0f)
                {
                    gFPS = gticknumber;
                    gticknumber = 0;
                    gtickdelta = 0.0f;
                }

                // Clear the current render buffer, should always be done before any rendering is handled.
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.ClearColor(Color.Black);

                // Set basic settings
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, ScreenWidth, ScreenHeight, 0, -50, 50);

                // Temporary for testing
                // Begin triangle rendering
                GL.Begin(PrimitiveType.Triangles);
                // Make a colorful triangle
                GL.Color3(Color.White);
                GL.Vertex2(X, Y);
                GL.Color3(Color.Red);
                GL.Vertex2(ScreenWidth - 2, 2);
                GL.Color3(Color.Green);
                GL.Vertex2(ScreenWidth - 2, ScreenHeight - 2);
                // End triangle rendering
                GL.End();

                // Render global text
                TextRenderer.Primary.RenderFinal();

                // Temporary for testing
                // Begin triangle rendering
                GL.Begin(PrimitiveType.Triangles);
                // Make a colorful triangle
                GL.Color3(Color.White);
                GL.Vertex2(X, Y);
                GL.Color3(Color.Red);
                GL.Vertex2(2, ScreenHeight - 2);
                GL.Color3(Color.Green);
                GL.Vertex2(ScreenWidth - 2, ScreenHeight - 2);
                // End triangle rendering
                GL.End();

                // Send the newly drawn code in, should always be done after all rendering is handled.
                PrimaryGameWindow.SwapBuffers();

                // Turn off first-graphics-draw mode: Always last!
                IsFirstGraphicsDraw = false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.GraphicsHandlers;
using mcmtestOpenTK.GraphicsHandlers.Text;
using mcmtestOpenTK.CommonHandlers;
using mcmtestOpenTK.GameplayHandlers.Entities;

namespace mcmtestOpenTK.GlobalHandler
{
    public partial class MainGame
    {
        static int gticknumber = 0;
        static double gtickdelta = 0;
        /// <summary>
        /// Called every render tick - should handle all graphics!
        /// </summary>
        /// <param name="sender">Irrelevant</param>
        /// <param name="e">Irrelevant</param>
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

                // Set to 3D mode
                GL.PushMatrix();
                setup3D();

                // Render all 3D graphics
                Standard3D();

                // End 3D
                end3D();
                GL.PopMatrix();

                // Set to begin 2D rendering
                GL.PushMatrix();
                Setup2D();

                // Render all 2D graphics
                Standard2D();

                // End 2D
                GL.PopMatrix();

                // Turn off first-graphics-draw mode: Always last!
                IsFirstGraphicsDraw = false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
            try
            {
                // Send the newly drawn code in, should always be done after all rendering is handled.
                PrimaryGameWindow.SwapBuffers();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }

        /// <summary>
        /// Sets the graphics for 2D screen rendering.
        /// </summary>
        public static void Setup2D()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, ScreenWidth, ScreenHeight, 0, -50, 50);
        }

        static Matrix4 Perspective;
        static Matrix4 View;

        /// <summary>
        /// Sets the graphics for 3D screen rendering.
        /// </summary>
        public static void setup3D()
        {
            // Initialise the projection view matrix
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // Setup a perspective view
            GL.MultMatrix(ref Perspective);
            View = Matrix4.LookAt(Player.player.Location, Player.player.Location + Forward, new Vector3(0, 0, 1));
            GL.MultMatrix(ref View);

            // Enable depth and culling
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        // Closes the 3D rendering mode.
        public static void end3D()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
        }

        /// <summary>
        /// Called every render frame to handle all 2D graphics.
        /// </summary>
        public static void Standard2D()
        {
            // Render global text
            GL.PushMatrix();
            TextRenderer.Primary.Draw();
            GL.PopMatrix();
        }

        /// <summary>
        /// Called every render frame to handle all 3D graphics.
        /// </summary>
        public static void Standard3D()
        {
            // Temporary for testing
            DrawCube(50, 0, 0, 0);
            DrawCube(100, 0, 0, 0);
            DrawCube(-50, 0, 0, 0);
            DrawCube(-100, 0, 0, 0);
            DrawCube(0, 50, 0, 0);
            DrawCube(0, 100, 0, 0);
            DrawCube(0, -50, 0, 0);
            DrawCube(0, -100, 0, 0);
        }

        /// <summary>
        /// Temporary for testing: Draw a cube model
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="ori"></param>
        static void DrawCube(float x, float y, float z, float ori)
        {
            GL.PushMatrix();

            GL.Translate(x, y, z);
            // GL.Rotate(ori, 0, 1, 0);

            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color.Orange);
            GL.Vertex3(0, 0, 0); GL.Vertex3(20, 0, 0);
            GL.Vertex3(20, 20, 0); GL.Vertex3(0, 20, 0);

            GL.Color3(Color.Red);
            GL.Vertex3(20, 0, 0); GL.Vertex3(20, 0, -20);
            GL.Vertex3(20, 20, -20); GL.Vertex3(20, 20, 0);

            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, -20);
            GL.Vertex3(20, 0, -20); GL.Vertex3(20, 0, 0);

            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, -20); GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 20, 0); GL.Vertex3(0, 20, -20);

            GL.Color3(Color.HotPink);
            GL.Vertex3(0, 20, 0); GL.Vertex3(20, 20, 0);
            GL.Vertex3(20, 20, -20); GL.Vertex3(0, 20, -20);

            GL.Color3(Color.Blue);
            GL.Vertex3(20, 0, -20); GL.Vertex3(0, 0, -20);
            GL.Vertex3(0, 20, -20); GL.Vertex3(20, 20, -20);

            GL.End();

            GL.PopMatrix();
        }
    }
}

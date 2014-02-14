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
                if (CurrentMouse.LeftButton == ButtonState.Pressed)
                {
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
                }

                // Temporary for testing
                // Line
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Black);
                GL.Vertex2(0, ScreenHeight / 2);
                GL.Color3(Color.Red);
                GL.Vertex2(movetestX, movetestY);
                GL.End();
                // Box
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(Color.Green);
                GL.Vertex2(movetestX - 2, movetestY - 2);
                GL.Vertex2(movetestX - 2, movetestY + 2);
                GL.Vertex2(movetestX + 2, movetestY + 2);
                GL.Vertex2(movetestX + 2, movetestY - 2);
                GL.End();
                
                // Temporary for testing
                GL.PushMatrix();
                NewRenderTry_Draw();
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

        // FOR THE TESTING REASONS

        static void DrawCube(float x, float y, float z, float ori)
        {
            GL.PushMatrix();

            GL.Translate(x, y, z);
            GL.Rotate(ori, 0, 1, 0);

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
        static void NewRenderTry_Init()
        {
            // Initialise the projection view matrix
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // Setup a perspective view
            float FOVradians = MathHelper.DegreesToRadians(45);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(FOVradians, (float)ScreenWidth / (float)ScreenHeight, 1, 4000);
            GL.MultMatrix(ref perspective);

            // Set the viewport to the whole window
            GL.Viewport(0, 0, ScreenWidth, ScreenHeight);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }
        static void NewRenderTry_Draw()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Rotate(movetestX, 0, 0, 1);
            GL.Rotate(movetestY, 0, 1, 0);

            // Draw a thing
            /*GL.Color3(Color.Orange);
            GL.Translate(50, 20, -200);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(20, 0, 0);
            GL.Vertex3(20, 20, 0);
            GL.Vertex3(0, 20, 0);
            GL.End();*/
            DrawCube(0, 0, -200, 0);
            DrawCube(100, 0, -200, 20);
            DrawCube(200, 0, -200, 0);
            DrawCube(300, 0, -200, 20);
        }
    }
}

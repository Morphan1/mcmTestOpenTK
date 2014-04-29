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
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GameplayHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
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
                GraphicsDeltaF = (float)GraphicsDelta;
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
                End2D();
                GL.PopMatrix();

                // Turn off first-graphics-draw mode: Always last!
                IsFirstGraphicsDraw = false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("MainGame/Render-1", ex);
            }
            try
            {
                // Send the newly drawn code in, should always be done after all rendering is handled.
                PrimaryGameWindow.SwapBuffers();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("MainGame/Render-2", ex);
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
            GL.Enable(EnableCap.Blend);
        }

        /// <summary>
        /// Closes the 2D rendering mode.
        /// </summary>
        public static void End2D()
        {
            GL.Disable(EnableCap.Blend);
        }

        static Matrix4 Perspective;
        static Matrix4 View;

        /// <summary>
        /// Sets the graphics for 3D screen rendering.
        /// </summary>
        public static void setup3D()
        {
            // Update the perspective matrix
            Perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(ClientCVar.g_fov.ValueF), (float)ScreenWidth / (float)ScreenHeight, 1, 4000);
            // Initialise the projection view matrix
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // Setup a perspective view
            GL.MultMatrix(ref Perspective);
            View = Matrix4.LookAt(Player.player.Position, Player.player.Position + Forward, new Vector3(0, 0, 1));
            GL.MultMatrix(ref View);

            // Enable depth and culling
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.UseProgram(Shader.Grayscale.Internal_Program);
            int uloc = GL.GetUniformLocation(Shader.Grayscale.Internal_Program, "mult_color");
            Shader.ColorMultShader.Bind();
            //Shader.ColorMultShader.SetColor(Color.FromArgb(255, 10, 10, 128));
            Hue += MainGame.GraphicsDeltaF * HueMult;
            if (Hue > 1 || Hue < 0)
            {
                HueMult *= -1;
                Hue += MainGame.GraphicsDeltaF * HueMult;
            }
            GL.Color4(Util.HSVtoRGB(Hue, 1, 1, 1));
        }

        public static float Hue = 0;
        public static float HueMult = 0.1f;

        /// <summary>
        /// Closes the 3D rendering mode.
        /// </summary>
        public static void end3D()
        {
            GL.UseProgram(0);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
        }

        /// <summary>
        /// Called every render frame to handle all 2D graphics.
        /// </summary>
        public static void Standard2D()
        {
            // Render debug text
            GLFont.DrawColoredText(debug);

            // Render console
            UIConsole.Draw();
        }

        /// <summary>
        /// Called every render frame to handle all 3D graphics.
        /// </summary>
        public static void Standard3D()
        {
            // Temporary for testing
            DrawCube(50, 0, 0);
            DrawCube(100, 0, 0);
            DrawCube(-50, 0, 0);
            DrawCube(-100, 0, 0);
            DrawCube(0, 50, 0);
            DrawCube(0, 100, 0);
            DrawCube(0, -50, 0);
            DrawCube(0, -100, 0);

            // Draw everything in the world
            DrawWorld();
        }

        /// <summary>
        /// Temporary for testing: Draw a cube model
        /// </summary>
        public static void DrawCube(float x, float y, float z, float angle = 0)
        {
            GL.PushMatrix();
            GL.Translate(x, y, z);
            GL.Rotate(angle, 0, 0, 1);

            Texture.Test.Bind();
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(20, 0, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(20, 20, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 20, 0);

            GL.TexCoord2(0, 0);
            GL.Vertex3(20, 0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(20, 0, -20);
            GL.TexCoord2(1, 1);
            GL.Vertex3(20, 20, -20);
            GL.TexCoord2(0, 1);
            GL.Vertex3(20, 20, 0);

            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(0, 0, -20);
            GL.TexCoord2(1, 1);
            GL.Vertex3(20, 0, -20);
            GL.TexCoord2(0, 1);
            GL.Vertex3(20, 0, 0);

            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, -20);
            GL.TexCoord2(1, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(0, 20, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 20, -20);

            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 20, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(20, 20, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(20, 20, -20);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 20, -20);

            GL.TexCoord2(0, 0);
            GL.Vertex3(20, 0, -20);
            GL.TexCoord2(1, 0);
            GL.Vertex3(0, 0, -20);
            GL.TexCoord2(1, 1);
            GL.Vertex3(0, 20, -20);
            GL.TexCoord2(0, 1);
            GL.Vertex3(20, 20, -20);

            GL.End();

            GL.PopMatrix();
        }
    }
}

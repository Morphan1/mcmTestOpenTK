using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.GameplayHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

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
            Texture shottext = null;
            uint FBO = 0;
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

                // Handle screenshotting
                if (Screenshot)
                {
                    shottext = Texture.CreateNullTexture(MainGame.ScreenWidth, MainGame.ScreenHeight);
                    GL.Ext.GenFramebuffers(1, out FBO);
                    GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FBO);
                    GL.Ext.FramebufferTexture2D(FramebufferTarget.DrawFramebuffer,
                        FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, shottext.Internal_Texture, 0);
                    GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                    GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FBO);
                    Screenshot = false;
                }

                // Clear the current render buffer, should always be done before any rendering is handled.
                GL.ClearColor(1, 0, 1, 1);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                // Set to 3D mode
                GL.PushMatrix();
                setup3D();

                // Render cartoon lines
                if (ClientCVar.r_cartoon.ValueB)
                {
                    Shader.Black.Bind();
                    IsWireFrame = true;
                    GL.PolygonMode(MaterialFace.Back, PolygonMode.Line);
                    GL.LineWidth(4);
                    GL.DepthFunc(DepthFunction.Lequal);
                    CullFace = CullFaceMode.Front;
                    GL.CullFace(CullFace);
                    Standard3D();
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    GL.LineWidth(1);
                    GL.DepthFunc(DepthFunction.Less);
                    IsWireFrame = false;
                    CullFace = CullFaceMode.Back;
                    GL.CullFace(CullFace);
                }

                // Render all 3D graphics
                if (ClientCVar.r_render3d.ValueB)
                {
                    Standard3D();
                }

                // Render wireframes
                if (ClientCVar.r_showwireframe.ValueB)
                {
                    if (ClientCVar.r_whitewireframe.ValueB)
                    {
                        Shader.White.Bind();
                        IsWireFrame = true;
                    }
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    GL.Disable(EnableCap.DepthTest);
                    Standard3D();
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    GL.Enable(EnableCap.DepthTest);
                    IsWireFrame = false;
                }

                // End 3D
                end3D();
                GL.PopMatrix();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("MainGame/Render-3D", ex);
            }
            try
            {
                // Set to begin 2D rendering
                GL.PushMatrix();
                Setup2D();

                // Render all 2D graphics
                Standard2D();

                if (shottext != null)
                {
                    GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                    Square sq = new Square();
                    sq.PositionLow = new Location(0, MainGame.ScreenHeight, 0);
                    sq.PositionHigh = new Location(MainGame.ScreenWidth, 0, 0);
                    sq.texture = shottext;
                    sq.Draw();
                    lock (ScreenshotLock)
                    {
                        Screenshots.Enqueue(shottext.SaveToBMP(true));
                    }
                }

                // End 2D
                End2D();
                GL.PopMatrix();

                // Turn off first-graphics-draw mode: Always last!
                IsFirstGraphicsDraw = false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("MainGame/Render-2D", ex);
            }
            try
            {
                // Send the newly drawn code in, should always be done after all rendering is handled.
                PrimaryGameWindow.SwapBuffers();

            }
            catch (Exception ex)
            {
                // Shouldn't happen, but we're keeping everything in TRY's, and need this separate from the main rendering.
                ErrorHandler.HandleError("MainGame/Render-Swap", ex);
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
            GL.Color4(1f, 1f, 1f, 1f);
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
            Perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(ClientCVar.r_fov.ValueF), (float)ScreenWidth / (float)ScreenHeight, 1, 4000);
            // Initialise the projection view matrix
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            // Setup a perspective view
            GL.MultMatrix(ref Perspective);
            if (ClientCVar.r_thirdperson.ValueB)
            {
                Location start = Player.player.Position + new Location(0, 0, Player.player.down ? 10: 16);
                Location target = start - Forward * 15;
                Location hitnormal;
                target = Collision.LineBox(start, target, new Location(-1, -1, -1), new Location(1, 1, 1), out hitnormal) - Forward;
                View = Matrix4.LookAt(Util.LocVec(target), Util.LocVec(start + Forward), new Vector3(0, 0, 1));
            }
            else
            {
                View = Matrix4.LookAt(Util.LocVec(Player.player.Position + new Location(0, 0, Player.player.down ? 8: 14)),
                    Util.LocVec(Player.player.Position + new Location(0, 0, Player.player.down ? 8 : 14) + Forward), new Vector3(0, 0, 1));
            }
            GL.MultMatrix(ref View);

            // Enable depth and culling
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            Shader.ColorMultShader.Bind();
            Hue += MainGame.GraphicsDeltaF * HueMult;
            if (Hue > 1 || Hue < 0)
            {
                HueMult *= -1;
                Hue += MainGame.GraphicsDeltaF * HueMult;
            }
            GL.Color4(Util.HSVtoRGB(Hue, 1, 1, 1));
            CullFace = CullFaceMode.Back;
            GL.CullFace(CullFace);
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
            GL.Color4(Color.White);

            // Draw the current screen
            Screen.Draw2D();

            if (Screen.Mode >= ScreenMode.MainMenu)
            {
                // Render console
                UIConsole.Draw();
            }
        }

        /// <summary>
        /// Called every render frame to handle all 3D graphics.
        /// </summary>
        public static void Standard3D()
        {
            // Draw the current screen
            Screen.Draw3D();
        }

        /// <summary>
        /// Temporary for testing: Draw a cube model
        /// </summary>
        public static void DrawCube(float x, float y, float z, float angle = 0, float scale = 10)
        {
            CubeModel model = new CubeModel(new Location(x, y, z), new Location(scale), Texture.Test);
            model.Draw();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using mcmtestOpenTK.Client.GameplayHandlers;
using OpenTK.Graphics.OpenGL;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class Skybox: Renderable
    {
        CubeModel model;

        /// <summary>
        /// Prepares the skybox for rendering.
        /// </summary>
        public void Init()
        {
            model = new CubeModel(Vector3.Zero, Vector3.One * 1000, Texture.Sky, Shader.Skyt);
        }

        public override void Draw()
        {
            GL.CullFace(CullFaceMode.Front);
            model.Position = new Vector3(Player.player.Position.X - 500, Player.player.Position.Y - 500, Player.player.Position.Z - 500);
            model.Draw();
            GL.CullFace(CullFaceMode.Back);
        }
    }
}

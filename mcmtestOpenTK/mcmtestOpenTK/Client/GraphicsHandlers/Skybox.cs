using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GameplayHandlers;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

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
            model = new CubeModel(Location.Zero, Location.One * 1000, Texture.Sky, Shader.Skyt);
        }

        public override void Draw()
        {
            GL.CullFace(MainGame.CullFace == CullFaceMode.Front ? CullFaceMode.Back: CullFaceMode.Front);
            model.Position = new Location(Player.player.Position.X - 500, Player.player.Position.Y - 500, Player.player.Position.Z - 500);
            model.Draw();
            GL.CullFace(MainGame.CullFace);
        }
    }
}

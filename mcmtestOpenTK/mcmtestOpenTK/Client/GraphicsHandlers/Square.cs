using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class Square: Renderable
    {
        /// <summary>
        /// Where the square is at on-screen (lower coordinates) (only X and Y are counted).
        /// </summary>
        public Location PositionLow;

        /// <summary>
        /// Where the square is at on-screen (higher coordinates) (only X and Y are counted).
        /// </summary>
        public Location PositionHigh;

        /// <summary>
        /// The shader to use.
        /// </summary>
        public Shader shader = null;

        /// <summary>
        /// The texture to use.
        /// </summary>
        public Texture texture = null;

        public Square()
        {
        }

        public override void Draw()
        {
            if (shader != null)
            {
                shader.Bind();
            }
            if (texture != null)
            {
                texture.Bind();
            }
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(PositionLow.X, PositionLow.Y);
            GL.TexCoord2(1, 0);
            GL.Vertex2(PositionHigh.X, PositionLow.Y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(PositionHigh.X, PositionHigh.Y);
            GL.TexCoord2(0, 1);
            GL.Vertex2(PositionLow.X, PositionHigh.Y);
            GL.End();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class CubeModel: Renderable
    {
        /// <summary>
        /// Where the cube is at (Min location).
        /// </summary>
        public Location Position;

        /// <summary>
        /// What size the cube is (Max - Min).
        /// </summary>
        public Location Scale;

        /// <summary>
        /// What angle to render at, if any.
        /// </summary>
        public float Angle = 0;

        /// <summary>
        /// The texture to use.
        /// </summary>
        public Texture texture = null;

        /// <summary>
        /// The shader to use, if any.
        /// </summary>
        public Shader shader = null;

        public CubeModel(Location _position, Location _scale, Texture _texture, Shader _shader = null)
        {
            Position = _position;
            Scale = _scale;
            texture = _texture;
            shader = _shader;
        }

        public override void Draw()
        {
            if (texture != null)
            {
                texture.Bind();
            }
            if (shader != null)
            {
                shader.Bind();
            }
            GL.PushMatrix();
            GL.Translate(Position.X, Position.Y, Position.Z);
            GL.Rotate(Angle, 0, 0, 1);
            GL.Scale(Scale.X, Scale.Y, -Scale.Z); // TODO: WHY IS Z NEGATIVE?!

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0); GL.Vertex3(0, 0, 0);
            GL.TexCoord2(1, 0); GL.Vertex3(1, 0, 0);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 0);
            GL.TexCoord2(0, 1); GL.Vertex3(0, 1, 0);

            GL.TexCoord2(0, 0); GL.Vertex3(1, 0, 0);
            GL.TexCoord2(1, 0); GL.Vertex3(1, 0, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(1, 1, 0);

            GL.TexCoord2(1, 0); GL.Vertex3(0, 0, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 0, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(1, 0, 0);
            GL.TexCoord2(0, 0); GL.Vertex3(0, 0, 0);

            GL.TexCoord2(0, 0); GL.Vertex3(0, 0, 1);
            GL.TexCoord2(1, 0); GL.Vertex3(0, 0, 0);
            GL.TexCoord2(1, 1); GL.Vertex3(0, 1, 0);
            GL.TexCoord2(0, 1); GL.Vertex3(0, 1, 1);

            GL.TexCoord2(0, 0); GL.Vertex3(0, 1, 0);
            GL.TexCoord2(1, 0); GL.Vertex3(1, 1, 0);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(0, 1, 1);

            GL.TexCoord2(0, 0); GL.Vertex3(1, 0, 1);
            GL.TexCoord2(1, 0); GL.Vertex3(0, 0, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(0, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(1, 1, 1);

            GL.End();

            GL.PopMatrix();
        }
    }
}

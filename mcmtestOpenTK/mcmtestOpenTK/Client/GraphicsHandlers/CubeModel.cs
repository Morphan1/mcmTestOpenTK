using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

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
        public double Angle = 0;

        /// <summary>
        /// The texture to use.
        /// </summary>
        public Texture texture = null;

        /// <summary>
        /// The shader to use, if any.
        /// </summary>
        public Shader shader = null;

        /// <summary>
        /// The horizontal scaling of the texture.
        /// </summary>
        public float Texture_HScale = 1;

        /// <summary>
        /// The vertical scaling of the texture.
        /// </summary>
        public float Texture_VScale = 1;

        /// <summary>
        /// The horizontal shift of the texture.
        /// </summary>
        public float Texture_HShift = 0;

        /// <summary>
        /// The vertical shift of the texture.
        /// </summary>
        public float Texture_VShift = 0;

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
            else
            {
                MainGame.GeneralShader.Bind();
            }
            GL.PushMatrix();
            GL.Translate(Position.X, Position.Y, Position.Z);
            GL.Rotate(Angle, 0, 0, 1);
            GL.Scale(Scale.X, Scale.Y, -Scale.Z); // TODO: WHY IS Z NEGATIVE?!

            float TexH0 = 0;
            float TexV0 = 0;
            double TexX1 = Scale.X / 10;
            double TexY1 = Scale.Y / 10;
            double TexZ1 = Scale.Z / 10;

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(TexH0, TexV0); GL.Vertex3(0, 0, 0);
            GL.TexCoord2(TexX1, TexV0); GL.Vertex3(1, 0, 0);
            GL.TexCoord2(TexX1, TexY1); GL.Vertex3(1, 1, 0);
            GL.TexCoord2(TexH0, TexY1); GL.Vertex3(0, 1, 0);

            GL.TexCoord2(TexH0, TexV0); GL.Vertex3(1, 0, 0);
            GL.TexCoord2(TexZ1, TexV0); GL.Vertex3(1, 0, 1);
            GL.TexCoord2(TexZ1, TexY1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(TexH0, TexY1); GL.Vertex3(1, 1, 0);

            GL.TexCoord2(TexZ1, TexV0); GL.Vertex3(0, 0, 1);
            GL.TexCoord2(TexZ1, TexX1); GL.Vertex3(1, 0, 1);
            GL.TexCoord2(TexH0, TexX1); GL.Vertex3(1, 0, 0);
            GL.TexCoord2(TexH0, TexV0); GL.Vertex3(0, 0, 0);

            GL.TexCoord2(TexH0, TexV0); GL.Vertex3(0, 0, 1);
            GL.TexCoord2(TexZ1, TexV0); GL.Vertex3(0, 0, 0);
            GL.TexCoord2(TexZ1, TexY1); GL.Vertex3(0, 1, 0);
            GL.TexCoord2(TexH0, TexY1); GL.Vertex3(0, 1, 1);

            GL.TexCoord2(TexH0, TexV0); GL.Vertex3(0, 1, 0);
            GL.TexCoord2(TexX1, TexV0); GL.Vertex3(1, 1, 0);
            GL.TexCoord2(TexX1, TexZ1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(TexH0, TexZ1); GL.Vertex3(0, 1, 1);

            GL.TexCoord2(TexH0, TexV0); GL.Vertex3(1, 0, 1);
            GL.TexCoord2(TexX1, TexV0); GL.Vertex3(0, 0, 1);
            GL.TexCoord2(TexX1, TexY1); GL.Vertex3(0, 1, 1);
            GL.TexCoord2(TexH0, TexY1); GL.Vertex3(1, 1, 1);

            GL.End();

            GL.PopMatrix();
        }
    }
}

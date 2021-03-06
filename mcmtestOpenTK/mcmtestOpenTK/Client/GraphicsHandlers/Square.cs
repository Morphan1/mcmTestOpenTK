﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

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

        /// <summary>
        /// Returns whether this square contains a given point.
        /// </summary>
        /// <param name="X">The X coordinate</param>
        /// <param name="Y">The Y coordinate</param>
        /// <returns>Whether it is contained</returns>
        public bool Contains(int X, int Y)
        {
            return PositionLow.X <= X && PositionLow.Y <= Y && PositionHigh.X >= X && PositionHigh.Y >= Y;
        }

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

        public static void DrawColoredSquare(int X, int Y, int X2, int Y2, Color color)
        {
            Square sq = new Square();
            sq.PositionLow.X = X;
            sq.PositionLow.Y = Y;
            sq.PositionHigh.X = X2;
            sq.PositionHigh.Y = Y2;
            sq.texture = Texture.White;
            sq.shader = Shader.ColorMultShader;
            GL.Color4(color);
            sq.Draw();
        }
    }
}

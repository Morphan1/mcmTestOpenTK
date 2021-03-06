﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.Collision;
using mcmtestOpenTK.Shared.Util;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class SimpleRenderer
    {
        /// <summary>
        /// Renders a plane.
        /// </summary>
        /// <param name="plane">The plane to render</param>
        public static void RenderPlane(Plane plane)
        {
            Location vec1 = plane.vec1;
            Location vec2 = plane.vec2;
            Location vec3 = plane.vec3;
            Location Normal = plane.Normal;
            RenderCustomPlane(vec1, vec2, vec3, Normal);
            Location middle = new Location((vec1.X + vec2.X + vec3.X) / 3, (vec1.Y + vec2.Y + vec3.Y) / 3, (vec1.Z + vec2.Z + vec3.Z) / 3);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(middle.X, middle.Y, middle.Z);
            GL.Vertex3(middle.X + Normal.X * 3, middle.Y + Normal.Y * 3, middle.Z + Normal.Z * 3);
            GL.End();
        }

        /// <summary>
        /// Renders a triangle plane.
        /// </summary>
        /// <param name="vec1">The first corner</param>
        /// <param name="vec2">The second corner</param>
        /// <param name="vec3">The third corner</param>
        public static void RenderCustomPlane(Location vec1, Location vec2, Location vec3, Location normal)
        {
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(0, 0);
            GL.Normal3(normal.X, normal.Y, normal.Z);
            GL.Vertex3(vec1.X, vec1.Y, vec1.Z);
            GL.TexCoord2(0, 1);
            GL.Normal3(normal.X, normal.Y, normal.Z);
            GL.Vertex3(vec2.X, vec2.Y, vec2.Z);
            GL.TexCoord2(1, 0);
            GL.Normal3(normal.X, normal.Y, normal.Z);
            GL.Vertex3(vec3.X, vec3.Y, vec3.Z);
            GL.End();
        }

        /// <summary>
        /// Renders an AABB (box).
        /// </summary>
        /// <param name="Box">The AABB to render</param>
        public static void RenderAABB(AABB Box)
        {
            Plane[] planes = Box.CalculateTriangles();
            for (int i = 0; i < planes.Length; i++)
            {
                RenderPlane(planes[i]);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class Texture
    {
        /// <summary>
        /// A full list of currently loaded textures.
        /// </summary>
        public static List<Texture> LoadedTextures = null;

        /// <summary>
        /// The internal OpenGL texture ID.
        /// </summary>
        public static uint Internal_Texture = 0;

        /// <summary>
        /// Starts or restarts the texture system.
        /// </summary>
        public static void InitTextureSystem()
        {
            if (LoadedTextures != null)
            {
                for (int i = 0; i < LoadedTextures.Count; i++)
                {
                    LoadedTextures[i].Remove();
                    i--;
                }
            }
            LoadedTextures = new List<Texture>();
        }

        /// <summary>
        /// Removes the texture from the system.
        /// </summary>
        public void Remove()
        {
            if (GL.IsTexture(Internal_Texture))
            {
                GL.DeleteTexture(Internal_Texture);
            }
            LoadedTextures.Remove(this);
        }
    }
}

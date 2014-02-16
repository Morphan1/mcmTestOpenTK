using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Shared;

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
        /// Loads a texture from file.
        /// </summary>
        /// <param name="filename">The name of the file to use</param>
        /// <returns>The loaded texture, or null if it does not exist.</returns>
        public static Texture LoadTexture(string filename)
        {
            try
            {
                if (!FileHandler.Exists(filename))
                {
                    ErrorHandler.HandleError("Cannot load texture, file '" +
                        TextStyle.Italic + TextStyle.White + filename + TextStyle.Reset + TextStyle.Error +
                        "' does not exist.");
                    return null;
                }
                return null; // TODO: Actually load
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                return null;
            }
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

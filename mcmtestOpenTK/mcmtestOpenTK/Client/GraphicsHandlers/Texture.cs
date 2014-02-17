using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
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
        /// A default white texture.
        /// </summary>
        public static Texture White = null;

        /// <summary>
        /// A default black texture.
        /// </summary>
        public static Texture Black = null;

        // This set: general preloaded common-use textures.
        public static Texture Test = null;
        public static Texture Console = null;

        public static Bitmap EmptyBitmap = null;
        public static Graphics GenericGraphicsObject = null;

        /// <summary>
        /// Starts or restarts the texture system.
        /// </summary>
        public static void InitTextureSystem()
        {
            // Dispose existing textures
            if (LoadedTextures != null)
            {
                for (int i = 0; i < LoadedTextures.Count; i++)
                {
                    LoadedTextures[i].Remove();
                    i--;
                }
            }
            EmptyBitmap = new Bitmap(1, 1);
            GenericGraphicsObject = Graphics.FromImage(EmptyBitmap);
            // Reset texture list
            LoadedTextures = new List<Texture>();
            // Pregenerate a few needed textures
            White = GenerateForColor(Color.White, "white");
            LoadedTextures.Add(White);
            Black = GenerateForColor(Color.Black, "Black");
            LoadedTextures.Add(Black);
            // Preload a few common textures
            Test = LoadTexture("common/test");
            LoadedTextures.Add(Test);
            Console = LoadTexture("common/console");
        }

        /// <summary>
        /// Gets the texture object for a specific texture name.
        /// </summary>
        /// <param name="texturename">The name of the texture</param>
        /// <returns>A valid texture object</returns>
        public static Texture GetTexture(string texturename)
        {
            texturename = FileHandler.CleanFileName(texturename);
            if (texturename.Length < 4 || texturename[texturename.Length - 4] != '.')
            {
                texturename = texturename + ".png";
            }
            for (int i = 0; i < LoadedTextures.Count; i++)
            {
                if (LoadedTextures[i].Name == texturename)
                {
                    return LoadedTextures[i];
                }
            }
            Texture Loaded = LoadTexture(texturename);
            if (Loaded != null)
            {
                LoadedTextures.Add(Loaded);
            }
            else
            {
                Loaded = new Texture();
                Loaded.Name = texturename;
                Loaded.Internal_Texture = White.Internal_Texture;
            }
            return Loaded;
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
                filename = FileHandler.CleanFileName(filename);
                if (filename.Length < 4 || filename[filename.Length - 4] != '.')
                {
                    filename = filename + ".png";
                }
                if (!FileHandler.Exists("textures/" + filename))
                {
                    ErrorHandler.HandleError("Cannot load texture, file '" +
                        TextStyle.Color_Standout + "textures/" + filename + TextStyle.Color_Error +
                        "' does not exist.");
                    return null;
                }
                Bitmap bmp = new Bitmap(FileHandler.ReadToStream("textures/" + filename));
                Texture texture = new Texture();
                texture.Name = filename;
                GL.GenTextures(1, out texture.Internal_Texture);
                GL.BindTexture(TextureTarget.Texture2D, texture.Internal_Texture);
                LockBitmapToTexture(bmp);
                bmp.Dispose();
                return texture;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Failed to load texture from filename '" +
                    TextStyle.Color_Standout + "textures/" + filename + TextStyle.Color_Error + "'", ex);
                return null;
            }
        }

        /// <summary>
        /// Creates a Texture object for a specific color.
        /// </summary>
        /// <param name="c">The color to use</param>
        /// <param name="name">The name of the texture</param>
        /// <returns>The generated texture</returns>
        public static Texture GenerateForColor(Color c, string name)
        {
            Texture texture = new Texture();
            texture.Name = name;
            GL.GenTextures(1, out texture.Internal_Texture);
            GL.BindTexture(TextureTarget.Texture2D, texture.Internal_Texture);
            Bitmap bmp = new Bitmap(2, 2);
            bmp.SetPixel(0, 0, c);
            bmp.SetPixel(0, 1, c);
            bmp.SetPixel(1, 0, c);
            bmp.SetPixel(1, 1, c);
            LockBitmapToTexture(bmp);
            bmp.Dispose();
            return texture;
        }

        /// <summary>
        /// Locks a bitmap file's data to a GL texture.
        /// </summary>
        /// <param name="bmp">The bitmap to use</param>
        public static void LockBitmapToTexture(Bitmap bmp)
        {
            // Send the bits across
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            bmp.UnlockBits(bmp_data);
            // Disable mipmapping
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        /// <summary>
        /// The full name of the texture.
        /// </summary>
        public string Name;

        /// <summary>
        /// The internal OpenGL texture ID.
        /// </summary>
        public uint Internal_Texture = 0;

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

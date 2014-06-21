using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
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
        public static Texture Sky = null;

        public static Bitmap EmptyBitmap = null;
        public static Graphics GenericGraphicsObject = null;

        /// <summary>
        /// The currently bound texture.
        /// </summary>
        public static uint Bound_Texture = 0;

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
            // Create a generic graphics object for later use
            EmptyBitmap = new Bitmap(1, 1);
            GenericGraphicsObject = Graphics.FromImage(EmptyBitmap);
            // Reset texture list
            LoadedTextures = new List<Texture>();
            // Pregenerate a few needed textures
            White = GenerateForColor(Color.White, "white");
            LoadedTextures.Add(White);
            Black = GenerateForColor(Color.Black, "black");
            LoadedTextures.Add(Black);
            Bound_Texture = 0;
            // Preload a few common textures
            Test = GetTexture("common/test");
            Console = GetTexture("common/console");
            Sky = GetTexture("common/sky");
        }

        /// <summary>
        /// Gets the texture object for a specific texture name.
        /// </summary>
        /// <param name="texturename">The name of the texture</param>
        /// <returns>A valid texture object</returns>
        public static Texture GetTexture(string texturename)
        {
            texturename = FileHandler.CleanFileName(texturename);
            for (int i = 0; i < LoadedTextures.Count; i++)
            {
                if (LoadedTextures[i].Name == texturename)
                {
                    return LoadedTextures[i];
                }
            }
            Texture Loaded = LoadTexture(texturename);
            if (Loaded == null)
            {
                Loaded = new Texture();
                Loaded.Name = texturename;
                Loaded.Internal_Texture = White.Original_InternalID;
                Loaded.Original_InternalID = White.Original_InternalID;
                Loaded.LoadedProperly = false;
            }
            LoadedTextures.Add(Loaded);
            return Loaded;
        }

        /// <summary>
        /// Loads a texture from file.
        /// </summary>
        /// <param name="filename">The name of the file to use</param>
        /// <returns>The loaded texture, or null if it does not exist</returns>
        private static Texture LoadTexture(string filename)
        {
            try
            {
                filename = FileHandler.CleanFileName(filename);
                if (!FileHandler.Exists("textures/" + filename + ".png"))
                {
                    ErrorHandler.HandleError("Cannot load texture, file '" +
                        TextStyle.Color_Standout + "textures/" + filename + ".png" + TextStyle.Color_Error +
                        "' does not exist.");
                    return null;
                }
                Bitmap bmp = new Bitmap(FileHandler.ReadToStream("textures/" + filename + ".png"));
                Texture texture = new Texture();
                texture.Name = filename;
                GL.GenTextures(1, out texture.Original_InternalID);
                texture.Internal_Texture = texture.Original_InternalID;
                texture.Bind();
                LockBitmapToTexture(bmp);
                texture.Width = bmp.Width;
                texture.Height = bmp.Height;
                bmp.Dispose();
                texture.LoadedProperly = true;
                return texture;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Failed to load texture from filename '" +
                    TextStyle.Color_Standout + "textures/" + filename + ".png" + TextStyle.Color_Error + "'", ex);
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
            GL.GenTextures(1, out texture.Original_InternalID);
            texture.Internal_Texture = texture.Original_InternalID;
            texture.Bind();
            texture.LoadedProperly = true;
            texture.Width = 2;
            texture.Height = 2;
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
        /// The texture that this texture was remapped to, if any.
        /// </summary>
        public Texture RemappedTo;

        /// <summary>
        /// The internal OpenGL texture ID.
        /// </summary>
        public uint Internal_Texture = 0;

        /// <summary>
        /// The original OpenGL texture ID that formed this texture.
        /// </summary>
        public uint Original_InternalID = 0;

        /// <summary>
        /// Whether the texture loaded properly.
        /// </summary>
        public bool LoadedProperly = false;

        /// <summary>
        /// The width of the texture.
        /// </summary>
        public int Width;

        /// <summary>
        /// The height of the texture.
        /// </summary>
        public int Height;

        /// <summary>
        /// Removes the texture from the system.
        /// </summary>
        public void Remove()
        {
            if (GL.IsTexture(Original_InternalID))
            {
                GL.DeleteTexture(Original_InternalID);
            }
            LoadedTextures.Remove(this);
        }

        /// <summary>
        /// Saves the texture to a file.
        /// </summary>
        /// <param name="filename">The name of the file to save to.</param>
        public void SaveToFile(string filename)
        {
            GL.BindTexture(TextureTarget.Texture2D, Original_InternalID);
            Bound_Texture = Original_InternalID;
            Bitmap bmp = new Bitmap(Width, Height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.GetTexImage(TextureTarget.Texture2D, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            DataStream ds = new DataStream();
            bmp.Save(ds, ImageFormat.Png);
            FileHandler.WriteBytes(filename + ".png", ds.ToArray());
        }

        /// <summary>
        /// Binds this texture to OpenGL.
        /// </summary>
        public void Bind()
        {
            if (Internal_Texture != Bound_Texture)
            {
                GL.BindTexture(TextureTarget.Texture2D, Internal_Texture);
            }
        }
    }
}

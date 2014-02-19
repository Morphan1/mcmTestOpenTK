using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using mcmtestOpenTK.Client.GraphicsHandlers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GraphicsHandlers.Text
{
    public class GLFont
    {
        /// <summary>
        /// The text file string to base letters on.
        /// </summary>
        public static string textfile;

        /// <summary>
        /// Loads the character list file.
        /// </summary>
        public static void LoadTextFile()
        {
            textfile = "";
            string[] datas = FileHandler.ReadText("info/characters.txt").Split('\n');
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].Length > 0 && !datas[i].StartsWith("//"))
                {
                    textfile += datas[i];
                }
            }
            string tempfile = "";
            for (int i = 0; i < textfile.Length; i++)
            {
                if (!tempfile.Contains(textfile[i]))
                {
                    tempfile += textfile[i].ToString();
                }
            }
            textfile = tempfile;
        }

        /// <summary>
        /// The texture containing all character images.
        /// </summary>
        public Texture BaseTexture;

        /// <summary>
        /// A list of all supported characters.
        /// </summary>
        public List<char> Characters;

        /// <summary>
        /// A list of all character locations on the base texture.
        /// </summary>
        public List<Rectangle> CharacterLocations;

        /// <summary>
        /// The name of the font.
        /// </summary>
        public string Name;

        /// <summary>
        /// The size of the font.
        /// </summary>
        public int Size;

        /// <summary>
        /// Whether the font is bold.
        /// </summary>
        public bool Bold;

        /// <summary>
        /// Whether the font is italic.
        /// </summary>
        public bool Italic;

        /// <summary>
        /// The font used to create this GLFont.
        /// </summary>
        public Font Internal_Font;

        public GLFont(Font font)
        {
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Name = font.Name;
            Size = (int)font.Size;
            Bold = font.Bold;
            Italic = font.Italic;
            Characters = new List<char>();
            CharacterLocations = new List<Rectangle>();
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap;
            Internal_Font = font;
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);
            Bitmap bmp = new Bitmap(1024, 1024);
            BaseTexture = new Texture();
            BaseTexture.Name = "font/" + font.Name.ToLower() + "/" + (font.Bold ? "b" : "") + (font.Italic ? "i" : "") + ((int)font.Size);
            GL.GenTextures(1, out BaseTexture.Internal_Texture);
            BaseTexture.Original_InternalID = BaseTexture.Internal_Texture;
            BaseTexture.Width = bmp.Width;
            BaseTexture.Height = bmp.Height;
            Texture.LoadedTextures.Add(BaseTexture);
            GL.BindTexture(TextureTarget.Texture2D, BaseTexture.Internal_Texture);
            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.Finish();
            bmp.UnlockBits(data);
            GL.BindTexture(TextureTarget.Texture2D, BaseTexture.Internal_Texture);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.Transparent);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                int X = 0;
                int Y = 0;
                int Height = font.Height;
                Brush brush = new SolidBrush(Color.White);
                for (int i = 0; i < textfile.Length; i++)
                {
                    string chr = textfile[i].ToString();
                    int nwidth = (int)Math.Ceiling(gfx.MeasureString(chr, font, new PointF(0, 0), sf).Width);
                    if (X + nwidth >= 1024)
                    {
                        Y += Height;
                        X = 0;
                    }
                    gfx.DrawString(chr, font, brush, new PointF(X, Y), sf);
                    Characters.Add(textfile[i]);
                    CharacterLocations.Add(new Rectangle(X, Y, nwidth, Height));
                    X += nwidth;
                }
            }
            data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bmp.Width, bmp.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.Dispose();
        }
    }
}

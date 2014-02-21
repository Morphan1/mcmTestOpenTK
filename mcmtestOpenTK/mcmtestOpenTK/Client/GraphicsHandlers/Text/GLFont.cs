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
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GraphicsHandlers.Text
{
    public class GLFont
    {
        /// <summary>
        /// The default font.
        /// </summary>
        public static GLFont Standard;

        /// <summary>
        /// A full list of loaded GLFonts.
        /// </summary>
        public static List<GLFont> Fonts;

        /// <summary>
        /// Prepares the font system.
        /// </summary>
        public static void Init()
        {
            if (Fonts != null)
            {
                for (int i = 0; i < Fonts.Count; i++)
                {
                    Fonts[i].Remove();
                    i--;
                }
            }
            // TODO: Dispose existing fonts?
            LoadTextFile();
            Fonts = new List<GLFont>();
            // Choose a default font: Segoe UI, Arial, Calibri, or generic.
            FontFamily[] families = FontFamily.Families;
            FontFamily family = FontFamily.GenericMonospace;
            int family_priority = 0;
            for (int i = 0; i < families.Length; i++)
            {
                if (family_priority < 10 && families[i].Name.ToLower() == "segoe ui")
                {
                    family = families[i];
                    family_priority = 10;
                }
                else if (family_priority < 5 && families[i].Name.ToLower() == "arial")
                {
                    family = families[i];
                    family_priority = 5;
                }
                else if (family_priority < 2 && families[i].Name.ToLower() == "calibri")
                {
                    family = families[i];
                    family_priority = 2;
                }
            }
            Font def = new Font(family, MainGame.FontSize);
            Standard = new GLFont(def);
            Fonts.Add(Standard);
        }

        /// <summary>
        /// Gets the font matching the specified settings.
        /// </summary>
        /// <param name="name">The name of the font</param>
        /// <param name="bold">Whether it's bold</param>
        /// <param name="italic">Whether it's italic</param>
        /// <param name="size">The font size</param>
        /// <returns>A valid font object</returns>
        public static GLFont GetFont(string name, bool bold, bool italic, int size)
        {
            string namelow = name.ToLower();
            for (int i = 0; i < Fonts.Count; i++)
            {
                if (Fonts[i].Name.ToLower() == namelow && bold == Fonts[i].Bold && italic == Fonts[i].Italic && size == Fonts[i].Size)
                {
                    return Fonts[i];
                }
            }
            GLFont Loaded = LoadFont(name, bold, italic, size);
            if (Loaded == null)
            {
                return Standard;
            }
            Fonts.Add(Loaded);
            return Loaded;
        }

        /// <summary>
        /// Loads a font matching the specified settings.
        /// </summary>
        /// <param name="name">The name of the font</param>
        /// <param name="bold">Whether it's bold</param>
        /// <param name="italic">Whether it's italic</param>
        /// <param name="size">The font size</param>
        /// <returns>A valid font object, or null if there was no match</returns>
        public static GLFont LoadFont(string name, bool bold, bool italic, int size)
        {
            Font font = new Font(name, size, (bold ? FontStyle.Bold : 0) | (italic ? FontStyle.Italic : 0));
            return new GLFont(font);
        }

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
            string[] datas = FileHandler.ReadText("info/characters.txt").Replace("\r", "").Split('\n');
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].Length > 0 && !datas[i].StartsWith("//"))
                {
                    textfile += datas[i];
                }
            }
            string tempfile = "?";
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
        public string Characters;

        /// <summary>
        /// A list of all character locations on the base texture.
        /// </summary>
        public List<RectangleF> CharacterLocations;

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
            CharacterLocations = new List<RectangleF>();
            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap;
            Internal_Font = font;
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);
            Bitmap bmp = new Bitmap(1024, 1024);
            BaseTexture = new Texture();
            BaseTexture.Name = "font/" + FileHandler.CleanFileName(font.Name) + "/" + (font.Bold ? "b" : "") + (font.Italic ? "i" : "") + ((int)font.Size);
            GL.GenTextures(1, out BaseTexture.Internal_Texture);
            BaseTexture.Original_InternalID = BaseTexture.Internal_Texture;
            BaseTexture.Width = bmp.Width;
            BaseTexture.Height = bmp.Height;
            Texture.LoadedTextures.Add(BaseTexture);
            BaseTexture.Bind();
            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.Finish();
            bmp.UnlockBits(data);
            BaseTexture.Bind();
            Characters = textfile;
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.Transparent);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                float X = 6;
                float Y = 0;
                float Height = font.Height;
                gfx.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, 5, (int)Height));
                Brush brush = new SolidBrush(Color.White);
                for (int i = 0; i < textfile.Length; i++)
                {
                    string chr = textfile[i] == '\t' ? "    ": textfile[i].ToString();
                    float nwidth = gfx.MeasureString(chr, font, new PointF(0, 0), sf).Width;
                    if (X + nwidth >= 1024)
                    {
                        Y += Height + 2;
                        X = 0;
                    }
                    gfx.DrawString(chr, font, brush, new PointF(X, Y), sf);
                    CharacterLocations.Add(new RectangleF(X, Y, nwidth, Height));
                    X += (float)Math.Ceiling(nwidth) + 2;
                }
            }
            data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bmp.Width, bmp.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.Dispose();
        }

        /// <summary>
        /// Removes the GLFont.
        /// </summary>
        public void Remove()
        {
            Fonts.Remove(this);
        }

        /// <summary>
        /// Gets the location of a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to find</param>
        /// <returns>A rectangle containing the precise location of a symbol</returns>
        public RectangleF RectForSymbol(char symbol)
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                if (Characters[i] == symbol)
                {
                    return CharacterLocations[i];
                }
            }
            return CharacterLocations[0];
        }

        /// <summary>
        /// Draws a single symbol at a specified location.
        /// </summary>
        /// <param name="symbol">The symbol to draw.</param>
        /// <param name="X">The X location to draw it at</param>
        /// <param name="Y">The Y location to draw it at</param>
        /// <returns>The length of the character in pixels</returns>
        private float DrawSingleCharacter(char symbol, float X, float Y, bool flip)
        {
            RectangleF rec = RectForSymbol(symbol);
            if (flip)
            {
                GL.TexCoord2(rec.X / 1024, rec.Y / 1024);
                GL.Vertex2(X, Y + rec.Height);
                GL.TexCoord2((rec.X + rec.Width) / 1024, rec.Y / 1024);
                GL.Vertex2(X + rec.Width, Y + rec.Height);
                GL.TexCoord2((rec.X + rec.Width) / 1024, (rec.Y + rec.Height) / 1024);
                GL.Vertex2(X + rec.Width, Y);
                GL.TexCoord2(rec.X / 1024, (rec.Y + rec.Height) / 1024);
                GL.Vertex2(X, Y);
            }
            else
            {
                GL.TexCoord2(rec.X / 1024, rec.Y / 1024);
                GL.Vertex2(X, Y);
                GL.TexCoord2((rec.X + rec.Width) / 1024, rec.Y / 1024);
                GL.Vertex2(X + rec.Width, Y);
                GL.TexCoord2((rec.X + rec.Width) / 1024, (rec.Y + rec.Height) / 1024);
                GL.Vertex2(X + rec.Width, Y + rec.Height);
                GL.TexCoord2(rec.X / 1024, (rec.Y + rec.Height) / 1024);
                GL.Vertex2(X, Y + rec.Height);
            }
            return rec.Width;
        }

        /// <summary>
        /// Draws a string at a specified location.
        /// </summary>
        /// <param name="str">The string to draw.</param>
        /// <param name="X">The X location to draw it at</param>
        /// <param name="Y">The Y location to draw it at</param>
        /// <returns>The length of the string in pixels</returns>
        public float DrawString(string str, float X, float Y, bool flip = false)
        {
            float nX = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\n')
                {
                    Y += Internal_Font.Height;
                    nX = 0;
                    Console.WriteLine("\n!");
                }
                nX += DrawSingleCharacter(str[i], X + nX, Y, flip);
            }
            return nX;
        }

        /// <summary>
        /// Measures the drawn length of a string.
        /// </summary>
        /// <param name="str">The string to measure</param>
        /// <returns>The length of the string</returns>
        public float MeasureString(string str)
        {
            float X = 0;
            for (int i = 0; i < str.Length; i++)
            {
                X += RectForSymbol(str[i]).Width;
            }
            return X;
        }

        public const int DefaultColor = 7;
        public static Color[] colors = new Color[] { 
            Color.FromArgb(0, 0, 0),      // 0  // 0 // Black
            Color.FromArgb(255, 0, 0),    // 1  // 1 // Red
            Color.FromArgb(0,255,0),      // 2  // 2 // Green
            Color.FromArgb(255, 255, 0),  // 3  // 3 // Yellow
            Color.FromArgb(0, 0, 255),    // 4  // 4 // Blue
            Color.FromArgb(0, 255, 255),  // 5  // 5 // Cyan
            Color.FromArgb(255, 0, 255),  // 6  // 6 // Magenta
            Color.FromArgb(255, 255, 255),// 7  // 7 // White
            Color.FromArgb(128,0,255),    // 8  // 8 // Purple
            Color.FromArgb(0, 128, 90),   // 9  // 9 // Torqoise
            Color.FromArgb(122, 77, 35),  // 10 // a // Brown
            Color.FromArgb(128, 0, 0),    // 11 // ! // DarkRed
            Color.FromArgb(0, 128, 0),    // 12 // @ // DarkGreen
            Color.FromArgb(128, 128, 0),  // 13 // # // DarkYellow
            Color.FromArgb(0, 0, 128),    // 14 // $ // DarkBlue
            Color.FromArgb(0, 128, 128),  // 15 // % // DarkCyan
            Color.FromArgb(128, 0, 128),  // 16 // - // DarkMagenta
            Color.FromArgb(128, 128, 128),// 17 // & // LightGray
            Color.FromArgb(64, 0, 128),   // 18 // * // DarkPurple
            Color.FromArgb(0, 64, 40),    // 19 // ( // DarkTorqoise
            Color.FromArgb(64, 64, 64),   // 20 // ) // DarkGray
            Color.FromArgb(61, 38, 17),   // 21 // A // DarkBrown
        };
        public static Point[] ShadowPoints = new Point[] {
            new Point(0, 1),
            new Point(0, 2),
            new Point(1, 0),
            new Point(1, 1),
            new Point(1, 2),
            new Point(2, 0),
            new Point(2, 1),
            new Point(2, 2),
        };
        public static Point[] EmphasisPoints = new Point[] {
            new Point(0, -1),
            new Point(0, 1),
            new Point(1, 0),
            new Point(-1, 0),
            new Point(0, -2),
            new Point(0, 2),
            new Point(2, 0),
            new Point(-2, 0),
            new Point(-1, -1),
            new Point(-1, 1),
            new Point(1, -1),
            new Point(1, 1),
        };

        /// <summary>
        /// Used to identify if an input character is a valid color symbol (generally the character that follows a '^'), for use by RenderColoredText
        /// </summary>
        /// <param name="c"><paramref name="c"/>The character to check</param>
        /// <returns>whether the character is a valid color symbol</returns>
        public static bool IsColorSymbol(char c)
        {
            return ((c >= '0' && c <= '9') /* 0123456789 */ ||
                    (c >= 'a' && c <= 'b') /* ab */ ||
                    (c >= 'd' && c <= 'f') /* def */ ||
                    (c >= 'h' && c <= 'l') /* hijkl */ ||
                    (c >= 'n' && c <= 'u') /* nopqrstu */ ||
                    (c >= 'R' && c <= 'T') /* RST */ ||
                    (c >= '#' && c <= '&') /* #$%& */ || // 35 - 38
                    (c >= '(' && c <= '*') /* ()* */ || // 40 - 42
                    (c == 'A') ||
                    (c == 'O') ||
                    (c == '-') || // 45
                    (c == '!') || // 33
                    (c == '@')    // 64
                   );
        }

        /// <summary>
        /// Correctly forms a Color object for the color number and transparency amount, for use by RenderColoredText
        /// </summary>
        /// <param name="color">The color number</param>
        /// <param name="trans">Transparency value, 0-255</param>
        /// <returns>A correctly formed color object</returns>
        public static Color ColorFor(int color, int trans)
        {
            return Color.FromArgb(trans, colors[color].R, colors[color].G, colors[color].B);
        }

        /// <summary>
        /// Fully renders colorful/fancy text (unless the text is not marked as fancy, or fancy rendering is disabled)
        /// </summary>
        /// <param name="text">The textual information to render</param>
        /// <param name="transmod">Transparency modifier (EG, 0.5 = half opacity) (0.0 - 1.0)</param>
        /// <param name="lockshadow">Whether to always have a drop-shadow</param>
        public static void DrawColoredText(PieceOfText text, float transmod = 1, bool lockshadow = false)
        {
            if (!text.fancy)
            {
                text.font.DrawString(text.Text, text.Position.X, text.Position.Y);
                return;
            }
            string[] lines = text.Text.Replace('\r', ' ').Replace(' ', (char)0x00A0).Replace("^q", "\"").Split('\n');
            int color = DefaultColor;
            int trans = (int)(255 * transmod);
            bool bold = false;
            bool italic = false;
            bool underline = false;
            bool strike = false;
            bool overline = false;
            bool highlight = false;
            bool emphasis = false;
            int ucolor = DefaultColor;
            int scolor = DefaultColor;
            int ocolor = DefaultColor;
            int hcolor = DefaultColor;
            int ecolor = DefaultColor;
            bool super = false;
            bool sub = false;
            bool flip = false;
            bool pseudo = false;
            bool jello = false;
            bool obfu = false;
            bool random = false;
            bool shadow = lockshadow;
            int otrans = (int)(255 * transmod);
            int etrans = (int)(255 * transmod);
            int htrans = (int)(255 * transmod);
            int strans = (int)(255 * transmod);
            int utrans = (int)(255 * transmod);
            float X = text.Position.X;
            int Y = text.Position.Y;
            GLFont font = text.font;
            font.BaseTexture.Bind();
            Shader.ColorMultShader.Bind();
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.Length == 0)
                {
                    Y += text.font.Internal_Font.Height;
                    continue;
                }
                int start = 0;
                for (int x = 0; x < line.Length; x++)
                {
                    if ((line[x] == '^' && x + 1 < line.Length && IsColorSymbol(line[x + 1])) || (x + 1 == line.Length))
                    {
                        string drawme = line.Substring(start, (x - start) + ((x + 1 < line.Length) ? 0 : 1));
                        start = x + 2;
                        x++;
                        if (drawme.Length > 0 && Y >= -font.Internal_Font.Height)
                        {
                            float width = font.MeasureString(drawme);
                            if (highlight)
                            {
                                DrawRectangle(X, Y, width, font.Internal_Font.Height, ColorFor(hcolor, htrans));
                            }
                            if (underline)
                            {
                                DrawRectangle(X, Y + (font.Internal_Font.Height * 4 / 5), width, 1, ColorFor(ucolor, utrans));
                            }
                            if (overline)
                            {
                                DrawRectangle(X, Y + 2, width, 1, ColorFor(ocolor, otrans));
                            }
                            if (shadow)
                            {
                                foreach (Point point in ShadowPoints)
                                {
                                    RenderBaseText(X + point.X, Y + point.Y, drawme, font, 0, 255, flip);
                                }
                            }
                            if (emphasis)
                            {
                                foreach (Point point in EmphasisPoints)
                                {
                                    RenderBaseText(X + point.X, Y + point.Y, drawme, font, ecolor, etrans, flip);
                                }
                            }
                            X += RenderBaseText(X, Y, drawme, font, color, trans, flip, pseudo, random, jello, obfu);
                            if (strike)
                            {
                                DrawRectangle(X, Y + (font.Internal_Font.Height / 2), width, 1, ColorFor(scolor, strans));
                            }
                        }
                        if (x < line.Length)
                        {
                            switch (line[x])
                            {
                                case '1': color = 1; break;
                                case '!': color = 11; break;
                                case '2': color = 2; break;
                                case '@': color = 12; break;
                                case '3': color = 3; break;
                                case '#': color = 13; break;
                                case '4': color = 4; break;
                                case '$': color = 14; break;
                                case '5': color = 5; break;
                                case '%': color = 15; break;
                                case '6': color = 6; break;
                                case '-': color = 16; break;
                                case '7': color = 7; break;
                                case '&': color = 17; break;
                                case '8': color = 8; break;
                                case '*': color = 18; break;
                                case '9': color = 9; break;
                                case '(': color = 19; break;
                                case '0': color = 0; break;
                                case ')': color = 20; break;
                                case 'a': color = 10; break;
                                case 'A': color = 21; break;
                                case 'i':
                                    {
                                        italic = true;
                                        GLFont nfont = (super || sub) ? (bold ? text.font_bolditalichalf : text.font_italichalf) : (bold ? text.font_bolditalic : text.font_italic);
                                        if (nfont != font)
                                        {
                                            GL.End();
                                            font = nfont;
                                            font.BaseTexture.Bind();
                                            GL.Begin(PrimitiveType.Quads);
                                        }
                                    }
                                    break;
                                case 'b':
                                    {
                                        bold = true;
                                        GLFont nfont = (super || sub) ? (italic ? text.font_bolditalichalf : text.font_boldhalf) : (italic ? text.font_bolditalic : text.font_bold);
                                        if (nfont != font)
                                        {
                                            GL.End();
                                            font = nfont;
                                            font.BaseTexture.Bind();
                                            GL.Begin(PrimitiveType.Quads);
                                        }
                                    }
                                    break;
                                case 'u': utrans = trans; underline = true; ucolor = color; break;
                                case 's': strans = trans; strike = true; scolor = color; break;
                                case 'h': htrans = trans; highlight = true; hcolor = color; break;
                                case 'e': etrans = trans; emphasis = true; ecolor = color; break;
                                case 'O': otrans = trans; overline = true; ocolor = color; break;
                                case 't': trans = (int)(128 * transmod); break;
                                case 'T': trans = (int)(64 * transmod); break;
                                case 'o': trans = (int)(255 * transmod); break;
                                case 'S':
                                    if (!super)
                                    {
                                        if (sub)
                                        {
                                            sub = false;
                                            Y -= text.font.Internal_Font.Height / 2;
                                        }
                                        GLFont nfont = bold && italic ? text.font_bolditalichalf : bold ? text.font_boldhalf : italic ? text.font_italichalf : text.font_half;
                                        if (nfont != font)
                                        {
                                            GL.End();
                                            font = nfont;
                                            font.BaseTexture.Bind();
                                            GL.Begin(PrimitiveType.Quads);
                                        }
                                    }
                                    super = true;
                                    break;
                                case 'l':
                                    if (!sub)
                                    {
                                        if (super)
                                        {
                                            super = false;
                                        }
                                        Y += text.font.Internal_Font.Height / 2;
                                        GLFont nfont = bold && italic ? text.font_bolditalichalf : bold ? text.font_boldhalf : italic ? text.font_italichalf : text.font_half;
                                        if (nfont != font)
                                        {
                                            GL.End();
                                            font = nfont;
                                            font.BaseTexture.Bind();
                                            GL.Begin(PrimitiveType.Quads);
                                        }
                                    }
                                    sub = true;
                                    break;
                                case 'd': shadow = true; break;
                                case 'j': /*if (!CVar.t_nojello.bvalue)*/ { jello = true; } break;
                                case 'k': /*if (!CVar.t_noobfu.bvalue)*/ { obfu = true; } break;
                                case 'R': /*if (!CVar.t_norandom.bvalue)*/ { random = true; } break;
                                case 'p': pseudo = true; break;
                                case 'f': flip = true; break;
                                case 'n':
                                    break;
                                case 'r':
                                    {
                                        GLFont nfont = text.font;
                                        if (nfont != font)
                                        {
                                            GL.End();
                                            font = nfont;
                                            font.BaseTexture.Bind();
                                            GL.Begin(PrimitiveType.Quads);
                                        }
                                        if (sub)
                                        {
                                            Y -= text.font.Internal_Font.Height / 2;
                                        }
                                        sub = false;
                                        super = false;
                                        flip = false;
                                        random = false;
                                        pseudo = false;
                                        jello = false;
                                        obfu = false;
                                        shadow = lockshadow;
                                        bold = false;
                                        italic = false;
                                        underline = false;
                                        strike = false;
                                        emphasis = false;
                                        highlight = false;
                                        trans = (int)(255 * transmod);
                                        overline = false;
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                    }
                }
                Y += text.font.Internal_Font.Height;
                X = 0;
            }
            GL.End();
            GL.UseProgram(0);
        }

        /// <summary>
        /// Semi-internal rendering of text strings.
        /// </summary>
        /// <param name="X">The X location to render at</param>
        /// <param name="Y">The Y location to render at</param>
        /// <param name="text">The text to render</param>
        /// <param name="font">The font to use</param>
        /// <param name="color">The color ID number to use</param>
        /// <param name="sf">The format</param>
        /// <param name="trans">Transparency</param>
        /// <param name="flip">Whether to flip the text</param>
        /// <param name="pseudo">Whether to use pseudo-random color</param>
        /// <param name="random">Whether to use real-random color</param>
        /// <param name="jello">Whether to use a jello effect</param>
        /// <param name="obfu">Whether to randomize letters</param>
        /// <returns>The length of the rendered text in pixels</returns>
        public static float RenderBaseText(float X, float Y, string text, GLFont font, int color,
            int trans = 255, bool flip = false, bool pseudo = false, bool random = false, bool jello = false, bool obfu = false)
        {
            if (obfu || pseudo || random || jello)
            {
                float nX = 0;
                for (int z = 0; z < text.Length; z++)
                {
                    char chr = text[z];
                    int col = color;
                    if (pseudo)
                    {
                        GL.Color4(ColorFor((chr % (colors.Length - 1)) + 1, trans));
                    }
                    if (random)
                    {
                        GL.Color4(ColorFor(Utilities.random.Next(colors.Length), trans));
                    }
                    if (obfu)
                    {
                        chr = (char)Utilities.random.Next(33, 126);
                    }
                    int iX = 0;
                    int iY = 0;
                    if (jello)
                    {
                        iX = Utilities.random.Next(-1, 1);
                        iY = Utilities.random.Next(-1, 1);
                    }
                    font.DrawSingleCharacter(chr, X + iX + nX, Y + iY, flip);
                    nX += font.RectForSymbol(text[z]).Width;
                }
                return nX;
            }
            else
            {
                GL.Color4(ColorFor(color, trans));
                float width = font.DrawString(text, X, Y, flip);
                return width;
            }
        }

        /// <summary>
        /// Measures fancy notated text strings.
        /// Note: Do not include newlines!
        /// </summary>
        /// <param name="line">The text to measure</param>
        /// <param name="text">The PieceOfText to get fonts from</param>
        /// <returns>the X-width of the text</returns>
        public static float MeasureFancyText(string line, PieceOfText text)
        {
            bool bold = false;
            bool italic = false;
            bool sub = false;
            float X = 0;
            GLFont font = text.font;
            int start = 0;
            line = line.Replace("^q", "\"").Replace("^n", "");
            for (int x = 0; x < line.Length; x++)
            {
                if ((line[x] == '^' && x + 1 < line.Length && IsColorSymbol(line[x + 1])) || (x + 1 == line.Length))
                {
                    string drawme = line.Substring(start, (x - start) + ((x + 1 < line.Length) ? 0 : 1));
                    start = x + 2;
                    x++;
                    if (drawme.Length > 0)
                    {
                        X += font.MeasureString(drawme);
                    }
                    if (x < line.Length)
                    {
                        switch (line[x])
                        {
                            case 'r':
                                font = text.font;
                                bold = false;
                                sub = false;
                                italic = false;
                                break;
                            case 'S':
                            case 'l':
                                if (!sub)
                                {
                                    font = bold && italic ? text.font_bolditalichalf : bold ? text.font_boldhalf : italic ? text.font_italichalf : text.font_half;
                                }
                                sub = true;
                                break;
                            case 'i':
                                italic = true;
                                font = (sub) ? (bold ? text.font_bolditalichalf : text.font_italichalf) : (bold ? text.font_bolditalic : text.font_italic);
                                break;
                            case 'b':
                                bold = true;
                                font = (sub) ? (italic ? text.font_bolditalichalf : text.font_boldhalf) : (italic ? text.font_bolditalic : text.font_bold);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return X;
        }

        /// <summary>
        /// Draws a rectangle to screen.
        /// </summary>
        /// <param name="X">The starting X</param>
        /// <param name="Y">The starting Y</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="c">The color to use</param>
        public static void DrawRectangle(float X, float Y, float width, float height, Color c)
        {
            GL.Color4(c);
            GL.TexCoord2(2f / 1024f, 2 / 1024f);
            GL.Vertex2(X, Y);
            GL.TexCoord2(4f / 1024f, 2f / 1024f);
            GL.Vertex2(X + width, Y);
            GL.TexCoord2(4f / 1024f, 4f / 1024f);
            GL.Vertex2(X + width, Y + height);
            GL.TexCoord2(2f / 1024f, 4f / 1024f);
            GL.Vertex2(X, Y + height);
        }
    }
}

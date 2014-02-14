using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.GlobalHandler;
using mcmtestOpenTK.CommonHandlers;

namespace mcmtestOpenTK.GraphicsHandlers
{
    class TextRenderer
    {
        public static TextRenderer Primary;
        public static Font DefaultFont;
        public int TextureID = -1;
        public Bitmap TextBitmap = null;
        public int Width = 0;
        public int Height = 0;
        public List<PieceOfText> texts = new List<PieceOfText>();
        public bool modified = true;
        public bool always_update = false;
        public bool IsInitted = false;

        /// <summary>
        /// Called once at startup time, to create the basic settings.
        /// </summary>
        public static void Init()
        {
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
            DefaultFont = new Font(family, MainGame.FontSize);
            Primary = new TextRenderer(MainGame.ScreenWidth, MainGame.ScreenHeight);
            Primary.always_update = true;
            Primary.ReInit();
        }

        /// <summary>
        /// Used to create a TextRender object.
        /// </summary>
        /// <param name="width">max width of the object</param>
        /// <param name="height">max height of the object</param>
        public TextRenderer(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Called whenever the window is modified, to readjust everything, generally from the render tick where possible.
        /// </summary>
        public void ReInit()
        {
            if (TextureID != -1)
            {
                GL.DeleteTexture(TextureID);
            }
            if (TextBitmap != null)
            {
                TextBitmap.Dispose();
            }
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);
            TextBitmap = new Bitmap(Width, Height);
            TextureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, TextureID);

            BitmapData data = TextBitmap.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.Finish();
            TextBitmap.UnlockBits(data);
            modified = true;
            IsInitted = true;
        }

        /// <summary>
        /// Called to add a new text object to be rendered.
        /// </summary>
        /// <param name="text">The text object to be rendered.</param>
        public void AddText(PieceOfText text)
        {
            modified = true;
            texts.Add(text);
        }

        /// <summary>
        /// Called to remove a text object.
        /// </summary>
        /// <param name="text">The location in the array of the text object to be removed.</param>
        public void RemoveText(int text)
        {
            modified = true;
            texts.RemoveAt(text);
        }

        /// <summary>
        /// Called to remove a text object.
        /// </summary>
        /// <param name="text">The text object to be removed.</param>
        public void RemoveText(PieceOfText text)
        {
            modified = true;
            texts.Remove(text);
        }

        /// <summary>
        /// Called whenever text is modified, from the render tick.
        /// </summary>
        public void RenderText()
        {
            if (!IsInitted || MainGame.IsFirstGraphicsDraw)
            {
                ReInit();
            }
            using (Graphics gfx = Graphics.FromImage(TextBitmap))
            {
                gfx.Clear(Color.Transparent);
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                for (int i = 0; i < texts.Count; i++)
                {
                    RenderColoredText(gfx, texts[i]);
                }
            }

            BitmapData data = TextBitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, Width, Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            TextBitmap.UnlockBits(data);
            modified = always_update;
        }

        /// <summary>
        /// Called by the render tick, to show the rendered text on the screen.
        /// </summary>
        public void RenderFinal()
        {
            if (modified || MainGame.IsFirstGraphicsDraw)
            {
                RenderText();
            }

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TextureID);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0); GL.Vertex2(TextBitmap.Width, 0);
            GL.TexCoord2(1, 1); GL.Vertex2(TextBitmap.Width, TextBitmap.Height);
            GL.TexCoord2(0, 1); GL.Vertex2(0, TextBitmap.Height);
            GL.End();
            GL.PopMatrix();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
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

        /// <summary>
        /// Used to identify if an input character is a valid color symbol (generally the character that follows a '^'), for use by RenderColoredText
        /// </summary>
        /// <param name="c"><paramref name="c"/>The character to check</param>
        /// <returns>whether the character is a valid color symbol</returns>
        public static bool IsColorSymbol(char c)
        {
            return ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'b') || (c == 'A') || (c == 'f') || (c == 'r') ||
                   (c == 'i') || (c == 'h') || (c == 'e') || (c == 't') || (c == 'T') || (c == 'o') || (c == 'O') ||
                   (c == '!') || (c == '@') || (c == 'l') || (c == 'd') || (c == 'S') || (c == 'k') || (c == 'j') ||
                   ((c >= '#') && (c <= '&')) /* #$%& */ || (c == '-') || (c == 'q') || (c == 'R') || (c == 'p') ||
                   ((c >= '(') && (c <= '*')) /* ()* */ || (c == 'u') || (c == 's'));
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
        /// <param name="graphics">The 2D graphics object to draw onto</param>
        /// <param name="text">The textual information to render</param>
        /// <param name="transmod">Transparency modifier (EG, 0.5 = half opacity) (0.0 - 1.0)</param>
        /// <param name="lockshadow">Whether to always have a drop-shadow</param>
        public static void RenderColoredText(Graphics graphics, PieceOfText text, float transmod = 1, bool lockshadow = false)
        {
            if (!text.fancy)
            {
                graphics.DrawString(text.Text, text.font, Brushes.White, text.Position);
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
            float X = 0;
            int Y = 0;
            float pX = 0;
            int pY = 0;
            Font font = text.font;
            StringFormat sf = StringFormat.GenericTypographic;
            graphics.TranslateTransform(text.Position.X, text.Position.Y);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int start = 0;
                for (int x = 0; x < line.Length; x++)
                {
                    if ((line[x] == '^' && x + 1 < line.Length && IsColorSymbol(line[x + 1])) || (x + 1 == line.Length))
                    {
                        string drawme = line.Substring(start, (x - start) + ((x + 1 < line.Length) ? 0 : 1));
                        start = x + 2;
                        x++;
                        if (drawme.Length > 0)
                        {
                            graphics.TranslateTransform(X - pX, Y - pY);
                            pX = X;
                            pY = Y;
                            float width = graphics.MeasureString(drawme, font, new PointF(0, 0), sf).Width;
                            if (highlight)
                            {
                                graphics.FillRectangle(new SolidBrush(ColorFor(hcolor, htrans)), new Rectangle(-1, 0, (int)width + 1, font.Height));
                            }
                            if (underline)
                            {
                                graphics.DrawLine(new Pen(ColorFor(ucolor, utrans), 1), new PointF(0, font.Height * 4 / 5), new PointF(width, font.Height * 4 / 5));
                            }
                            if (overline)
                            {
                                graphics.DrawLine(new Pen(ColorFor(ocolor, otrans), 1), new Point(0, 2), new Point((int)width, 2));
                            }
                            if (shadow)
                            {
                                for (int shadX = 0; shadX < 3; shadX++)
                                {
                                    for (int shadY = 1; shadY < 3; shadY++)
                                    {
                                        graphics.DrawString(drawme, font, new SolidBrush(Color.Black), new PointF(shadX, shadY), sf);
                                    }
                                }
                            }
                            if (emphasis)
                            {
                                for (int empX = -1; empX < 2; empX += 2)
                                {
                                    for (int empY = -1; empY < 2; empY += 2)
                                    {
                                        graphics.DrawString(drawme, font, new SolidBrush(ColorFor(ecolor, etrans)), new PointF(empX, empY), sf);
                                    }
                                }
                            }
#if SYSTEM_FONT_HANDLING
                            if (obfu || pseudo || random || jello) // Must be handled manually regardless of settings.
                            {
#endif
                            for (int z = 0; z < drawme.Length; z++)
                            {
                                char chr = drawme[z];
                                int col = color;
                                if (pseudo)
                                {
                                    col = chr % colors.Length;
                                }
                                if (random)
                                {
                                    col = Util.random.Next(colors.Length);
                                }
                                if (obfu)
                                {
                                    chr = (char)Util.random.Next(33, 126);
                                }
                                int iX = 0;
                                int iY = 0;
                                if (jello)
                                {
                                    iX = Util.random.Next(-1, 1);
                                    iY = Util.random.Next(-1, 1);
                                }
                                graphics.DrawString(chr.ToString(), font, new SolidBrush(ColorFor(col, trans)), new PointF(iX, iY), sf);
                                float size = graphics.MeasureString(drawme[z].ToString(), font, new PointF(0, 0), sf).Width;
                                X += size;
                                pX += size;
                                graphics.TranslateTransform(size, 0);
                            }
#if SYSTEM_FONT_HANDLING
                            }
                            else
                            {
                                // Spaces strings differently depending on character count... meaning all the text will shift
                                // if you add characters to the end. Which is bad.
                                graphics.DrawString(drawme, font, new SolidBrush(ColorFor(color, trans)), new PointF(0, 0), sf);
                                X += width;
                            }
#endif
                            if (strike)
                            {
                                graphics.DrawLine(new Pen(ColorFor(scolor, strans), 1), new PointF(0, font.Height * 0.5f), new PointF(width, font.Height * 0.5f));
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
                                    italic = true;
                                    font = (super || sub) ? (bold ? text.font_bolditalichalf : text.font_italichalf) : (bold ? text.font_bolditalic : text.font_italic);
                                    break;
                                case 'b':
                                    bold = true;
                                    font = (super || sub) ? (italic ? text.font_bolditalichalf : text.font_boldhalf) : (italic ? text.font_bolditalic : text.font_bold);
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
                                            graphics.TranslateTransform(0, -text.font.Height / 2);
                                        }
                                        font = bold && italic ? text.font_bolditalichalf : bold ? text.font_boldhalf : italic ? text.font_italichalf : text.font_half;
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
                                        graphics.TranslateTransform(0, text.font.Height / 2);
                                        font = bold && italic ? text.font_bolditalichalf : bold ? text.font_boldhalf : italic ? text.font_italichalf : text.font_half;
                                    }
                                    sub = true;
                                    break;
                                case 'd': shadow = true; break;
                                case 'j': /*if (!CVar.t_nojello.bvalue)*/ { jello = true; } break;
                                case 'k': /*if (!CVar.t_noobfu.bvalue)*/ { obfu = true; } break;
                                case 'R': /*if (!CVar.t_norandom.bvalue)*/ { random = true; } break;
                                case 'p': pseudo = true; break;
                                case 'f':
                                    if (!flip)
                                    {
                                        flip = true;
                                        graphics.ScaleTransform(1, -1);
                                        graphics.TranslateTransform(0, -text.font.Height);
                                        if (X == 0)
                                        {
                                            Y += -2 * text.font.Height;
                                        }
                                    }
                                    break;
                                case 'r':
                                    font = text.font;
                                    if (flip)
                                    {
                                        graphics.ScaleTransform(1, -1);
                                        graphics.TranslateTransform(0, -text.font.Height);
                                        flip = false;
                                        if (X == 0)
                                        {
                                            Y += 2 * text.font.Height;
                                        }
                                    }
                                    if (sub)
                                    {
                                        graphics.TranslateTransform(0, -text.font.Height / 2);
                                    }
                                    sub = false;
                                    super = false;
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
                                default:
                                    break;
                            }
                        }
                    }
                }
                Y += (flip ? -1: 1) * text.font.Height;
                X = 0;
            }
            graphics.Transform = new Matrix();
        }
    }
}

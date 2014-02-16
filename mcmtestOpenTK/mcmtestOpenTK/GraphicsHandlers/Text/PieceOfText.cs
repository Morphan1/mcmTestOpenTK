using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace mcmtestOpenTK.GraphicsHandlers.Text
{
    public class PieceOfText
    {
        public string Text = "";
        public Font font;
        public Font font_bold;
        public Font font_italic;
        public Font font_bolditalic;
        public Font font_half;
        public Font font_boldhalf;
        public Font font_italichalf;
        public Font font_bolditalichalf;
        public Point Position;
        public bool fancy;

        /// <summary>
        /// Creates a new PieceOfText
        /// </summary>
        /// <param name="txt">The text</param>
        /// <param name="pos">X/Y screen coordinates</param>
        /// <param name="fnt">The font to use... leave out to specify the global default font</param>
        /// <param name="fncy">Whether to apply fancy text rendering to this text</param>
        public PieceOfText(string txt, Point pos, Font fnt = null, bool fncy = true)
        {
            Text = txt;
            Position = pos;
            fancy = fncy;
            font = fnt == null ? TextRenderer.DefaultFont : fnt;
            font_bold = new Font(font, FontStyle.Bold);
            font_italic = new Font(font, FontStyle.Italic);
            font_bolditalic = new Font(font, FontStyle.Bold | FontStyle.Italic);
            font_half = new Font(font.FontFamily, font.Size / 2);
            font_boldhalf = new Font(font_bold.FontFamily, font.Size / 2, FontStyle.Bold);
            font_italichalf = new Font(font_bold.FontFamily, font.Size / 2, FontStyle.Italic);
            font_bolditalichalf = new Font(font_bold.FontFamily, font.Size / 2, FontStyle.Bold | FontStyle.Italic);
        }
    }
}

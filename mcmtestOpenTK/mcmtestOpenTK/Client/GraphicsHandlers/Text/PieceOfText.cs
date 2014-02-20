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

namespace mcmtestOpenTK.Client.GraphicsHandlers.Text
{
    public class PieceOfText
    {
        public string Text = "";
        public GLFont font;
        public GLFont font_bold;
        public GLFont font_italic;
        public GLFont font_bolditalic;
        public GLFont font_half;
        public GLFont font_boldhalf;
        public GLFont font_italichalf;
        public GLFont font_bolditalichalf;
        public Point Position;
        public bool fancy;

        /// <summary>
        /// Creates a new PieceOfText
        /// </summary>
        /// <param name="txt">The text</param>
        /// <param name="pos">X/Y screen coordinates</param>
        /// <param name="fnt">The font to use... leave out to specify the global default font</param>
        /// <param name="fncy">Whether to apply fancy text rendering to this text</param>
        public PieceOfText(string txt, Point pos, GLFont fnt = null, bool fncy = true)
        {
            Text = txt;
            Position = pos;
            fancy = fncy;
            font = fnt == null ? GLFont.Standard : fnt;
            font_bold = GLFont.GetFont(font.Name, true, false, font.Size);
            font_italic = GLFont.GetFont(font.Name, false, true, font.Size);
            font_bolditalic = GLFont.GetFont(font.Name, true, true, font.Size);
            font_half = GLFont.GetFont(font.Name, false, false, font.Size / 2);
            font_boldhalf = GLFont.GetFont(font.Name, true, false, font.Size / 2);
            font_italichalf = GLFont.GetFont(font.Name, false, true, font.Size / 2);
            font_bolditalichalf = GLFont.GetFont(font.Name, true, true, font.Size / 2);
        }
    }
}

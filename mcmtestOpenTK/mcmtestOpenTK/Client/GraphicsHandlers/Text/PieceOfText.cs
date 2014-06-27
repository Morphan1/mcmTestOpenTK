using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GraphicsHandlers.Text
{
    public class PieceOfText
    {
        public string Text = "";
        public FontSet set;
        public Location Position;
        public bool fancy;

        /// <summary>
        /// Creates a new PieceOfText
        /// </summary>
        /// <param name="txt">The text</param>
        /// <param name="pos">X/Y screen coordinates</param>
        /// <param name="fnt">The font to use... leave out to specify the global default font</param>
        /// <param name="fncy">Whether to apply fancy text rendering to this text</param>
        public PieceOfText(string txt, Location pos, FontSet fnt = null, bool fncy = true)
        {
            Text = txt;
            Position = pos;
            fancy = fncy;
            if (fnt == null)
            {
                set = FontSet.Standard;
            }
            else
            {
                set = fnt;
            }
        }
    }
}

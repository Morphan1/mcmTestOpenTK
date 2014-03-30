using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.GraphicsHandlers.Text
{
    public class FontSet
    {
        /// <summary>
        /// The general font used for all normal purposes.
        /// </summary>
        public static FontSet Standard;

        /// <summary>
        /// A list of all currently loaded font sets.
        /// </summary>
        public static List<FontSet> Fonts = new List<FontSet>();

        /// <summary>
        /// Prepares the FontSet system.
        /// </summary>
        public static void Init()
        {
            Standard = new FontSet("standard");
            Standard.Load(GLFont.Standard.Name, GLFont.Standard.Size);
            Fonts.Add(Standard);
        }

        /// <summary>
        /// Gets a font by a specified name.
        /// </summary>
        /// <param name="fontname">The name of the font</param>
        /// <param name="fontsize">The size of the font</param>
        /// <returns>The specified font</returns>
        public static FontSet GetFont(string fontname, int fontsize)
        {
            string namelow = fontname.ToLower();
            for (int i = 0; i < Fonts.Count; i++)
            {
                if (Fonts[i].font.Size == fontsize && Fonts[i].Name == namelow)
                {
                    return Fonts[i];
                }
            }
            FontSet toret = new FontSet(fontname);
            toret.Load(fontname, fontsize);
            Fonts.Add(toret);
            return toret;
        }

        public GLFont font;
        public GLFont font_bold;
        public GLFont font_italic;
        public GLFont font_bolditalic;
        public GLFont font_half;
        public GLFont font_boldhalf;
        public GLFont font_italichalf;
        public GLFont font_bolditalichalf;

        public string Name;

        public FontSet(string _name)
        {
            Name = _name.ToLower();
        }

        public void Load(string fontname, int fontsize)
        {
            font = GLFont.GetFont(fontname, false, false, fontsize);
            font_bold = GLFont.GetFont(fontname, true, false, fontsize);
            font_italic = GLFont.GetFont(fontname, false, true, fontsize);
            font_bolditalic = GLFont.GetFont(fontname, true, true, fontsize);
            font_half = GLFont.GetFont(fontname, false, false, fontsize / 2);
            font_boldhalf = GLFont.GetFont(fontname, true, false, fontsize / 2);
            font_italichalf = GLFont.GetFont(fontname, false, true, fontsize / 2);
            font_bolditalichalf = GLFont.GetFont(fontname, true, true, fontsize / 2);
        }
    }
}

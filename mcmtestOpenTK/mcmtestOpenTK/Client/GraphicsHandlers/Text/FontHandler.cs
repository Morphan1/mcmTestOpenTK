using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GraphicsHandlers.Text
{
    public class FontHandler
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
            // TODO: Dispose existing fonts?
            GLFont.LoadTextFile();
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
    }
}

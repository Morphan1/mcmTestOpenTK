using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared
{
    public class TextStyle
    {
        public static string Default = "^r^7";
        public static string Reset = "^r";
        public static string Bold = "^b";
        public static string Italic = "^i";
        public static string Transparent = "^t";
        public static string Opaque = "^o";
        public static string White = "^7";
        public static string Color_Simple = "^r^7";
        public static string Color_Standout = "^r^0^h^5";
        public static string Color_Readable = "^r^7^e^0^b";
        public static string Color_Chat = "^r^2^d";
        public static string Color_Error = "^r^0^h^3";
        public static string Color_Commandhelp = "^r^0^h^1";
        public static string Color_Separate = "^r^5";
        public static string Color_Outgood = "^r^2";
        public static string Color_Outbad = "^r^1";
        public static string Color_Importantinfo = "^r^3";

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
    }
}

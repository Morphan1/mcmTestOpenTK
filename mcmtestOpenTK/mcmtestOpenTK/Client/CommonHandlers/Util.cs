using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using mcmtestOpenTK.Shared;
using OpenTK;

namespace mcmtestOpenTK.Client.CommonHandlers
{
    class Util
    {
        /// <summary>
        /// Converts a Hue/Saturation/Value color to a standard RGB color.
        /// </summary>
        /// <param name="hue">The hue value, 0.0 - 1.0</param>
        /// <param name="saturation">The saturation value, 0.0 - 1.0</param>
        /// <param name="value">The brightness value, 0.0 - 1.0</param>
        /// <param name="alpha">The alpha value, 0.0 - 1.0</param>
        /// <returns>A valid color object</returns>
        public static Color HSVtoRGB(float hue, float saturation, float value, float alpha)
        {
            while (hue > 1f) { hue -= 1f; }
            while (hue < 0f) { hue += 1f; }
            while (saturation > 1f) { saturation -= 1f; }
            while (saturation < 0f) { saturation += 1f; }
            while (value > 1f) { value -= 1f; }
            while (value < 0f) { value += 1f; }
            if (hue > 0.999f) { hue = 0.999f; }
            if (hue < 0.001f) { hue = 0.001f; }
            if (saturation > 0.999f) { saturation = 0.999f; }
            if (saturation < 0.001f) { return Color.FromArgb((int)(alpha * 255f), (int)(value * 255f), (int)(value * 255f), (int)(value * 255f)); }
            if (value > 0.999f) { value = 0.999f; }
            if (value < 0.001f) { value = 0.001f; }

            float h6 = hue * 6f;
            if (h6 == 6f) { h6 = 0f; }
            int ihue = (int)(h6);
            float p = value * (1f - saturation);
            float q = value * (1f - (saturation * (h6 - (float)ihue)));
            float t = value * (1f - (saturation * (1f - (h6 - (float)ihue))));
            switch (ihue)
            {
                case 0:
                    return Color.FromArgb((int)(alpha * 255f), (int)(value * 255f), (int)(t * 255f), (int)(p * 255f));
                case 1:
                    return Color.FromArgb((int)(alpha * 255f), (int)(q * 255f), (int)(value * 255f), (int)(p * 255f));
                case 2:
                    return Color.FromArgb((int)(alpha * 255f), (int)(p * 255f), (int)(value * 255f), (int)(t * 255f));
                case 3:
                    return Color.FromArgb((int)(alpha * 255f), (int)(p * 255f), (int)(q * 255f), (int)(value * 255f));
                case 4:
                    return Color.FromArgb((int)(alpha * 255f), (int)(t * 255f), (int)(p * 255f), (int)(value * 255f));
                default:
                    return Color.FromArgb((int)(alpha * 255f), (int)(value * 255f), (int)(p * 255f), (int)(q * 255f));
            }
        }

        public static Vector3 LocVec(Location loc)
        {
            return new Vector3(loc.X, loc.Y, loc.Z);
        }
    }
}

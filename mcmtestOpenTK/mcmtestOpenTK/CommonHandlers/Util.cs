using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace mcmtestOpenTK.CommonHandlers
{
    class Util
    {
        public static Random random;
        /// <summary>
        /// Called once at system begin to prepare the Utilities.
        /// </summary>
        public static void Init()
        {
            random = new Random();
        }

        /// <summary>
        /// Returns a one-length vector of the Yaw/Pitch angle input.
        /// </summary>
        /// <param name="yaw">The yaw angle, in radians.</param>
        /// <param name="pitch">The pitch angle, in radians.</param>
        /// <returns></returns>
        public static Vector3 ForwardVector(float yaw, float pitch)
        {
            float cp = (float)Math.Cos(pitch);
            Vector3 toret;
            toret.X = -(cp * (float)Math.Cos((double)yaw));
            toret.Y = -(cp * (float)Math.Sin((double)yaw));
            toret.Z = ((float)Math.Sin((double)pitch));
            return toret;
        }

        /// <summary>
        /// Rotates a vector by a certain yaw.
        /// </summary>
        /// <param name="vec">The original vector.</param>
        /// <param name="yaw">The yaw to rotate by.</param>
        /// <returns>The rotated yaw.</returns>
        public static Vector2 RotateVector(Vector2 vec, float yaw)
        {
            float cos = (float)Math.Cos(yaw);
            float sin = (float)Math.Sin(yaw);
            return new Vector2((vec.X * cos) - (vec.Y * sin), (vec.X * sin) + (vec.Y * cos));
        }
    }
}

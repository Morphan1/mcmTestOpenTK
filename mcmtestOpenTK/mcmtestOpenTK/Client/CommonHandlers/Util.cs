﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace mcmtestOpenTK.Client.CommonHandlers
{
    class Util
    {
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
        /// Converts a forward vector to yaw/pitch angles.
        /// </summary>
        /// <param name="input">The forward vector</param>
        /// <returns>The yaw/pitch angle vector</returns>
        public static Vector2 VectorToAngles(Vector3 input)
        {
            if (input.X == 0 && input.Y == 0)
            {
                if (input.Z > 0)
                {
                    return new Vector2(0, 90);
                }
                else
                {
                    return new Vector2(0, 270);
                }
            }
            else
            {
                float yaw;
                float pitch;
                if (input.X != 0)
                {
                    yaw = (float)(Math.Atan2(input.Y, input.X) * 180 / Math.PI) + 180;
                }
                else if (input.Y > 0)
                {
                    yaw = 90;
                }
                else
                {
                    yaw = 270;
                }
                pitch = (float)(Math.Atan2(input.Z, Math.Sqrt(input.X * input.X + input.Y * input.Y)) * 180 / Math.PI);
                while (pitch < -180)
                {
                    pitch += 360;
                }
                while (pitch > 180)
                {
                    pitch -= 360;
                }
                while (yaw < 0)
                {
                    yaw += 360;
                }
                while (yaw > 360)
                {
                    yaw -= 360;
                }
                return new Vector2(yaw, pitch);
            }
        }

        /// <summary>
        /// Rotates a vector by a certain yaw.
        /// </summary>
        /// <param name="vec">The original vector.</param>
        /// <param name="yaw">The yaw to rotate by.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector2 RotateVector(Vector2 vec, float yaw)
        {
            float cos = (float)Math.Cos(yaw);
            float sin = (float)Math.Sin(yaw);
            return new Vector2((vec.X * cos) - (vec.Y * sin), (vec.X * sin) + (vec.Y * cos));
        }

        /// <summary>
        /// Rotates a vector by a certain yaw and pitch.
        /// </summary>
        /// <param name="vec">The original vector.</param>
        /// <param name="yaw">The yaw to rotate by.</param>
        /// <param name="pitch">The pitch to rotate by.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3 RotateVector(Vector3 vec, float yaw, float pitch)
        {
            float cosyaw = (float)Math.Cos(yaw);
            float cospitch = (float)Math.Cos(pitch);
            float sinyaw = (float)Math.Sin(yaw);
            float sinpitch = (float)Math.Sin(pitch);
            float bX = vec.Z * sinpitch + vec.X * cospitch;
            float bZ = vec.Z * cospitch - vec.X * sinpitch;
            return new Vector3(bX * cosyaw - vec.Y * sinyaw, bX * sinyaw + vec.Y * cosyaw, bZ);
        }
    }
}

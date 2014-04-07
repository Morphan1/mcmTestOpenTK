﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared
{
    class Utilities
    {
        public static Random random;
        /// <summary>
        /// Called once at system begin to prepare the Utilities.
        /// </summary>
        public static void Init()
        {
            random = new Random();
            SysConsole.Init();
        }

        /// <summary>
        /// Returns a string representation of the specified time.
        /// </summary>
        /// <returns>The time as a string</returns>
        public static string DateTimeToString(DateTime dt)
        {
            string utcoffset = "";
            DateTime UTC = dt.ToUniversalTime();
            if (dt.CompareTo(UTC) < 0)
            {
                TimeSpan span = UTC.Subtract(dt);
                utcoffset = "-" + Pad(((int)Math.Floor(span.TotalHours)).ToString(), '0', 2) + ":" + Pad(span.Minutes.ToString(), '0', 2);
            }
            else
            {
                TimeSpan span = dt.Subtract(UTC);
                utcoffset = "+" + Pad(((int)Math.Floor(span.TotalHours)).ToString(), '0', 2) + ":" + Pad(span.Minutes.ToString(), '0', 2);
            }
            return Pad(dt.Year.ToString(), '0', 4) + "/" + Pad(dt.Month.ToString(), '0', 2) + "/" +
                Pad(dt.Day.ToString(), '0', 2) + " " + Pad(dt.Hour.ToString(), '0', 2) + ":" +
                Pad(dt.Minute.ToString(), '0', 2) + ":" + Pad(dt.Second.ToString(), '0', 2) + " UTC" + utcoffset;
        }

        /// <summary>
        /// Pads a string to a specified length with a specified input, on a specified side.
        /// </summary>
        /// <param name="input">The original string</param>
        /// <param name="padding">The symbol to pad with</param>
        /// <param name="length">How far to pad it to</param>
        /// <param name="left">Whether to pad left (true), or right (false)</param>
        /// <returns>The padded string</returns>
        public static string Pad(string input, char padding, int length, bool left = true)
        {
            int targetlength = length - input.Length;
            StringBuilder pad = new StringBuilder(targetlength);
            for (int i = 0; i < targetlength; i++)
            {
                pad.Append(padding);
            }
            if (left)
            {
                return pad + input;
            }
            else
            {
                return input + pad;
            }
        }

        /// <summary>
        /// Returns a peice of text copied a specified number of times.
        /// </summary>
        /// <param name="text">What text to copy</param>
        /// <param name="times">How many times to copy it</param>
        /// <returns></returns>
        public static string CopyText(string text, int times)
        {
            StringBuilder toret = new StringBuilder(text.Length * times);
            for (int i = 0; i < times; i++)
            {
                toret.Append(text);
            }
            return toret.ToString();
        }

        /// <summary>
        /// Returns the number of times a character occurs in a string.
        /// </summary>
        /// <param name="input">The string containing the character</param>
        /// <param name="countme">The character which the string contains</param>
        /// <returns>How many times the character occurs</returns>
        public static int CountCharacter(string input, char countme)
        {
            int count = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == countme)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Combines a list of strings into a single string, separated by spaces.
        /// </summary>
        /// <param name="input">The list of strings to combine</param>
        /// <param name="start">The index to start from</param>
        /// <returns>The combined string</returns>
        public static string Concat(List<string> input, int start = 0)
        {
            StringBuilder output = new StringBuilder();
            for (int i = start; i < input.Count; i++)
            {
                output.Append(input[i]).Append(" ");
            }
            return (output.Length > 0 ? output.ToString().Substring(0, output.Length - 1) : "");
        }

        /// <summary>
        /// Converts a string value to the long-integer value it represents.
        /// Returns 0 if the string does not represent a long-integer.
        /// </summary>
        /// <param name="input">The string to get the value from</param>
        /// <returns>a long-integer value</returns>
        public static long StringToLong(string input)
        {
            long output = 0;
            long.TryParse(input, out output);
            return output;
        }

        /// <summary>
        /// Converts a string value to the integer value it represents.
        /// Returns 0 if the string does not represent an integer.
        /// </summary>
        /// <param name="input">The string to get the value from</param>
        /// <returns>an integer value</returns>
        public static int StringToInt(string input)
        {
            int output = 0;
            int.TryParse(input, out output);
            return output;
        }

        /// <summary>
        /// Converts a string value to the double value it represents.
        /// Returns 0 if the string does not represent an double.
        /// </summary>
        /// <param name="input">The string to get the value from</param>
        /// <returns>a double value</returns>
        public static double StringToDouble(string input)
        {
            double output = 0;
            double.TryParse(input, out output);
            return output;
        }

        /// <summary>
        /// Converts a string value to the float value it represents.
        /// Returns 0 if the string does not represent an float.
        /// </summary>
        /// <param name="input">The string to get the value from</param>
        /// <returns>a float value</returns>
        public static float StringToFloat(string input)
        {
            float output = 0;
            float.TryParse(input, out output);
            return output;
        }
    }
}

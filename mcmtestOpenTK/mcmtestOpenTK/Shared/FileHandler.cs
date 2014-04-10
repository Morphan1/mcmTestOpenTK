using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace mcmtestOpenTK.Shared
{
    public class FileHandler
    {
        /// <summary>
        /// The default text encoding.
        /// </summary>
        public static Encoding encoding = new UTF8Encoding(false);

        /// <summary>
        /// The base directory in which all data is stored.
        /// </summary>
        public static string BaseDirectory = Environment.CurrentDirectory.Replace("\\", "/") + "/data/";

        /// <summary>
        /// Cleans a file name for direct system calls.
        /// </summary>
        /// <param name="input">The original file name</param>
        /// <returns>The cleaned file name</returns>
        public static string CleanFileName(string input)
        {
            StringBuilder output = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                // Remove double slashes, or "./"
                if ((input[i] == '/' || input[i] == '\\') && output.Length > 0 && (output[output.Length - 1] == '/' || output[output.Length - 1] == '.'))
                {
                    continue;
                }
                // Fix backslashes to forward slashes for cross-platform folders
                if (input[i] == '\\')
                {
                    output.Append('/');
                    continue;
                }
                // Remove ".." (up-a-level) folders, or "/."
                if (input[i] == '.' && output.Length > 0 && (output[output.Length - 1] == '.' || output[output.Length - 1] == '/'))
                {
                    continue;
                }
                // Clean spaces to underscores
                if (input[i] == (char)0x00A0 || input[i] == ' ')
                {
                    output.Append('_');
                    continue;
                }
                // Remove non-ASCII symbols, ASCII control codes, and Windows control symbols
                if (input[i] < 32 || input[i] > 126 || input[i] == '?' ||
                    input[i] == ':' || input[i] == '*' || input[i] == '|' ||
                    input[i] == '"' || input[i] == '<' || input[i] == '>')
                {
                    output.Append('_');
                    continue;
                }
                // Lower-case letters only
                if (input[i] >= 'A' && input[i] <= 'Z')
                {
                    output.Append((char)(input[i] - ('A' - 'a')));
                    continue;
                }
                // All others normal
                output.Append(input[i]);
            }
            // Also, trim leading/trailing spaces.
            return output.ToString().Trim();
        }

        /// <summary>
        /// Returns whether a file exists.
        /// </summary>
        /// <param name="filename">The name of the file to look for</param>
        /// <returns>Whether the file exists</returns>
        public static bool Exists(string filename)
        {
            return File.Exists(BaseDirectory + CleanFileName(filename));
        }

        /// <summary>
        /// Returns all the byte data in a file.
        /// </summary>
        /// <param name="filename">The name of the file to read</param>
        /// <returns>The file's data, as a byte array</returns>
        public static byte[] ReadBytes(string filename)
        {
            return File.ReadAllBytes(BaseDirectory + CleanFileName(filename));
        }

        /// <summary>
        /// Returns a stream of the byte data in a file.
        /// </summary>
        /// <param name="filename">The name of the file to read</param>
        /// <returns>The file's data, as a stream</returns>
        public static DataStream ReadToStream(string filename)
        {
            return new DataStream(ReadBytes(filename));
        }

        /// <summary>
        /// Returns all the text data in a file.
        /// </summary>
        /// <param name="filename">The name of the file to read</param>
        /// <returns>The file's data, as a string</returns>
        public static string ReadText(string filename)
        {
            return encoding.GetString(ReadBytes(filename)).Replace('\r', ' ');
        }

        /// <summary>
        /// Writes bytes to a file.
        /// </summary>
        /// <param name="filename">The name of the file to write to</param>
        /// <param name="bytes">The byte data to write</param>
        public static void WriteBytes(string filename, byte[] bytes)
        {
            string fname = CleanFileName(filename);
            string dir = Path.GetDirectoryName(BaseDirectory + fname);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllBytes(BaseDirectory + fname, bytes);
        }

        /// <summary>
        /// Writes text to a file.
        /// </summary>
        /// <param name="filename">The name of the file to write to</param>
        /// <param name="text">The text data to write</param>
        public static void WriteText(string filename, string text)
        {
            WriteBytes(filename, encoding.GetBytes(text.Replace('\r', ' ')));
        }

        /// <summary>
        /// Adds text to a file/
        /// </summary>
        /// <param name="filename">The name of the file to add to</param>
        /// <param name="text">The text data to add</param>
        public static void AppendText(string filename, string text)
        {
            string fname = CleanFileName(filename);
            string dir = Path.GetDirectoryName(BaseDirectory + fname);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.AppendAllText(BaseDirectory + fname, text.Replace('\r', ' '), encoding);
        }

        /// <summary>
        /// Compresses a byte array using the GZip algorithm.
        /// </summary>
        /// <param name="input">Uncompressed data</param>
        /// <returns>Compressed data</returns>
        public static byte[] GZip(byte[] input)
        {
            MemoryStream memstream = new MemoryStream();
            var GZStream = new GZipStream(memstream, CompressionMode.Compress);
            GZStream.Write(input, 0, input.Length);
            GZStream.Close();
            byte[] finaldata = memstream.ToArray();
            memstream.Close();
            return finaldata;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.Networking
{
    /// <summary>
    /// The NetStringManager tracks strings that need to be transmitted over the network.
    /// Used to shorten packet lengths by using an integer ID instead of sending the full string.
    /// Mainly for things like texture/shader/sound/... names, not chat or the like.
    /// </summary>
    public class NetStringManager
    {
        /// <summary>
        /// All strings the client is aware of, mapped by ID.
        /// </summary>
        public static Dictionary<int, string> Strings;
        
        /// <summary>
        /// Prepares the network string manager.
        /// </summary>
        public static void Init()
        {
            Strings = new Dictionary<int,string>();
            Strings.Add(0, "");
        }
        
        /// <summary>
        /// Gets the string corresponding to an ID, or an empty string if there's no match.
        /// </summary>
        /// <param name="ID">The ID of the string</param>
        /// <returns>The string for the ID, or empty</returns>
        public static string GetStringForID(int ID)
        {
            string result;
            if (Strings.TryGetValue(ID, out result))
            {
                return result;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Adds a string to the list of strings the client knows.
        /// </summary>
        /// <param name="ID">The ID of the string</param>
        /// <param name="text">The string itself</param>
        public static void AddString(int ID, string text)
        {
            if (Strings.ContainsKey(ID))
                Strings.Remove(ID);
            Strings.Add(ID, text);
        }
    }
}

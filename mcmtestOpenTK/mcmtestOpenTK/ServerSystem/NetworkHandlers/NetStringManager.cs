using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers
{
    /// <summary>
    /// The NetStringManager tracks strings that need to be transmitted over the network.
    /// Used to shorten packet lengths by using an integer ID instead of sending the full string.
    /// Mainly for things like texture/shader/sound/... names, not chat or the like.
    /// </summary>
    public class NetStringManager
    {
        /// <summary>
        /// All strings that have already defined IDs.
        /// </summary>
        public static List<string> Strings;

        /// <summary>
        /// Prepares the network string manager.
        /// </summary>
        public static void Init()
        {
            Strings = new List<string>();
            Strings.Add("");
        }

        /// <summary>
        /// Gets the string corresponding to an ID, or an empty string if there's no match.
        /// </summary>
        /// <param name="ID">The ID of the string</param>
        /// <returns>The string for the ID, or empty</returns>
        public static string GetStringForID(int ID)
        {
            if (ID < 0 || ID >= Strings.Count)
            {
                return "";
            }
            return Strings[ID];
        }

        /// <summary>
        /// Gets or creates the ID for a string.
        /// </summary>
        /// <param name="text">The string to find</param>
        /// <returns>A valid ID</returns>
        public static int GetStringID(string text)
        {
            for (int i = 0; i < Strings.Count; i++)
            {
                if (Strings[i] == text)
                {
                    return i;
                }
            }
            return CreateID(text);
        }

        /// <summary>
        /// Creates an ID for a specified string.
        /// </summary>
        /// <param name="text">The string to make an ID for</param>
        /// <returns>The new ID</returns>
        public static int CreateID(string text)
        {
            Strings.Add(text);
            AnnounceStringID(Strings.Count - 1);
            return Strings.Count - 1;
        }

        /// <summary>
        /// Announce a new string ID to all online players.
        /// </summary>
        /// <param name="ID">The ID to announce</param>
        public static void AnnounceStringID(int ID)
        {
            NetworkBase.SendToAllPlayers(new NetstringPacketOut(ID, GetStringForID(ID)));
        }

        /// <summary>
        /// Announce all string IDs to a specific player.
        /// </summary>
        /// <param name="ID">The player to announce to</param>
        public static void AnnounceAll(Player player)
        {
            for (int i = 1; i < Strings.Count; i++)
            {
                player.Send(new NetstringPacketOut(i, Strings[i]));
            }
        }
    }
}

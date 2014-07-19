using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Shared.Util;

namespace mcmtestOpenTK.Client.GameplayHandlers
{
    class Item
    {
        /// <summary>
        /// The name used to spawn this item.
        /// </summary>
        public string Name;

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// A description of the item.
        /// </summary>
        public string Description;

        /// <summary>
        /// Whether this item can be thrown.
        /// </summary>
        public bool CanThrow;

        /// <summary>
        /// How many of the item there are here.
        /// </summary>
        public int Quantity;

        /// <summary>
        /// What texture this item uses.
        /// </summary>
        public string Texture;

        /// <summary>
        /// What shader to use when rendering this item.
        /// </summary>
        public string Shader;

        /// <summary>
        /// How heavy this item is.
        /// </summary>
        public float Weight;

        /// <summary>
        /// How much space this item takes up.
        /// </summary>
        public float Volume;

        public Item(string _name)
        {
            Name = _name.ToLower();
            DisplayName = null;
            Description = null;
            Quantity = 1;
            CanThrow = true;
        }

        /// <summary>
        /// Converts a byte array to an item.
        /// </summary>
        /// <param name="data">The byte array</param>
        /// <returns>A valid item</returns>
        public static Item FromBytes(byte[] data)
        {
            if (data.Length < 4 + 4 + 4 + 4 + 4 + 4 + 1 + 4 + 4)
            {
                return null;
            }
            int pos = 0;
            // Name (int)
            int name = BitConverter.ToInt32(data, pos);
            Item toret = new Item(NetStringManager.GetStringForID(name));
            pos += 4;
            // Weight (float)
            toret.Weight = BitConverter.ToSingle(data, pos);
            pos += 4;
            // Volume (float)
            toret.Volume = BitConverter.ToSingle(data, pos);
            pos += 4;
            // Texture (int)
            toret.Texture = NetStringManager.GetStringForID(BitConverter.ToInt32(data, pos));
            if (toret.Texture == "")
            {
                toret.Texture = null;
            }
            pos += 4;
            // Shader (int)
            toret.Shader = NetStringManager.GetStringForID(BitConverter.ToInt32(data, pos));
            if (toret.Shader == "")
            {
                toret.Shader = null;
            }
            pos += 4;
            // Quantity (int)
            toret.Volume = BitConverter.ToInt32(data, pos);
            pos += 4;
            // CanThrow (byte)
            toret.CanThrow = data[pos] == 1;
            pos += 1;
            // Displayname length (int)
            int dnameLength = BitConverter.ToInt32(data, pos);
            pos += 4;
            // displayname
            if (data.Length < pos + dnameLength)
            {
                return null;
            }
            toret.DisplayName = FileHandler.encoding.GetString(data, pos, dnameLength);
            pos += dnameLength;
            // Description length (int)
            int descLength = BitConverter.ToInt32(data, pos);
            pos += 4;
            // Description
            if (data.Length < pos + descLength)
            {
                return null;
            }
            toret.DisplayName = FileHandler.encoding.GetString(data, pos, descLength);
            return toret;
        }
    }
}

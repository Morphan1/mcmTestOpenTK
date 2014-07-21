using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Client.GraphicsHandlers;

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
        /// What texture to use when rendering this item.
        /// </summary>
        public Texture texture;

        /// <summary>
        /// What shader to use when rendering this item.
        /// </summary>
        public Shader shader;

        /// <summary>
        /// What model to use when rendering this item.
        /// </summary>
        public Model model;

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

        public override string ToString()
        {
            string sname = shader == null ? "null" : shader.Name;
            return "ITEM:{name:" + Name + ",weight:" + Weight + ",volume:" + Volume + ",quantity:" + Quantity + ",model:" + model.Name
                + ",texture:" + texture.Name + ",shader:" + sname + ",displayname:" + DisplayName + ",description:" + Description + "}";
        }

        /// <summary>
        /// Draws the item at a given location with a given rotation.
        /// </summary>
        /// <param name="loc">The location to draw it at</param>
        /// <param name="rot">The rotation to draw it with</param>
        public void Draw(Location loc, Location rot)
        {
            texture.Bind();
            if (shader != null)
            {
                shader.Bind();
            }
            model.Draw(loc, rot, Location.One * 0.1f);
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
            string texture = NetStringManager.GetStringForID(BitConverter.ToInt32(data, pos));
            if (texture == "")
            {
                toret.texture = null;
            }
            else
            {
                toret.texture = Texture.GetTexture(texture);
            }
            pos += 4;
            // Shader (int)
            string shader = NetStringManager.GetStringForID(BitConverter.ToInt32(data, pos));
            if (shader == "")
            {
                toret.shader = null;
            }
            else
            {
                toret.shader = Shader.GetShader(shader);
            }
            pos += 4;
            // Model (int)
            string model = NetStringManager.GetStringForID(BitConverter.ToInt32(data, pos));
            if (model == "")
            {
                toret.model = null;
            }
            else
            {
                toret.model = Model.GetModel(model);
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

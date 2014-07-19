using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}

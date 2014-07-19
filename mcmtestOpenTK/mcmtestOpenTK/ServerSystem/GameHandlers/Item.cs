using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.ServerSystem.GameHandlers
{
    public class Item
    {
        /// <summary>
        /// The name used to spawn this item.
        /// </summary>
        public string Name;

        /// <summary>
        /// All items this item inheritted traits from.
        /// </summary>
        public List<Item> Inheritance;

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
            Name = _name;
            DisplayName = null;
            Description = null;
            Quantity = 1;
            CanThrow = true;
            Inheritance = new List<Item>();
        }

        /// <summary>
        /// Called when the item is 'used'.
        /// </summary>
        public virtual void OnUse()
        {
        }

        /// <summary>
        /// Called when the item is 'clicked'.
        /// </summary>
        public virtual void OnClick()
        {
        }

        /// <summary>
        /// Called when the item is thrown.
        /// </summary>
        public virtual void OnThrow()
        {
        }

        /// <summary>
        /// Returns an exact copy of this item.
        /// </summary>
        /// <returns>The copy</returns>
        public virtual Item Duplicate(Item it = null)
        {
            Item item = it == null ? it : new Item(Name);
            item.Inheritance = new List<Item>(Inheritance);
            item.DisplayName = DisplayName;
            item.Description = Description;
            item.Quantity = Quantity;
            item.CanThrow = CanThrow;
            item.Texture = Texture;
            item.Shader = Shader;
            item.Weight = Weight;
            item.Volume = Volume;
            return item;
        }
    }
}

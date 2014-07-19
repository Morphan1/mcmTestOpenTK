using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.Shared.Util;

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
            Name = _name.ToLower();
            DisplayName = null;
            Description = null;
            Quantity = 1;
            CanThrow = true;
            Inheritance = new List<Item>();
        }

        /// <summary>
        /// Called when the item is 'used'.
        /// </summary>
        public virtual void OnUse(Player player)
        {
        }

        /// <summary>
        /// Called when the item is 'clicked'.
        /// </summary>
        public virtual void OnClick(Player player)
        {
        }

        /// <summary>
        /// Called when the item is thrown.
        /// </summary>
        public virtual void OnThrow(Player player)
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
            item.Name = Name;
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

        public byte[] GetBytes()
        {
            byte[] DName = FileHandler.encoding.GetBytes(DisplayName);
            byte[] Desc = FileHandler.encoding.GetBytes(Description);
            byte[] toret = new byte[4 + 4 + 4 + 4 + 4 + 4 + 1 + 4 + DName.Length + 4 + Desc.Length];
            // Name (int)
            int pos = 0;
            BitConverter.GetBytes(NetStringManager.GetStringID(Name)).CopyTo(toret, pos);
            pos += 4;
            // Weight (float)
            BitConverter.GetBytes(Weight).CopyTo(toret, pos);
            pos += 4;
            // Volume (float)
            BitConverter.GetBytes(Volume).CopyTo(toret, pos);
            pos += 4;
            // Texture (int)
            BitConverter.GetBytes(NetStringManager.GetStringID(Texture == null ? "": Texture)).CopyTo(toret, pos);
            pos += 4;
            // Shader (int)
            BitConverter.GetBytes(NetStringManager.GetStringID(Shader == null ? "": Shader)).CopyTo(toret, pos);
            pos += 4;
            // Quantity (int)
            BitConverter.GetBytes(Quantity).CopyTo(toret, pos);
            pos += 4;
            // CanThrow (byte)
            toret[pos] = (byte)(CanThrow ? 1 : 0);
            pos += 1;
            // Displayname length (int)
            BitConverter.GetBytes(DName.Length).CopyTo(toret, pos);
            pos += 4;
            // displayname
            DName.CopyTo(toret, pos);
            pos += DName.Length;
            // Description length (int)
            BitConverter.GetBytes(Desc.Length).CopyTo(toret, pos);
            pos += 4;
            // Description
            Desc.CopyTo(toret, pos);
            return toret;
        }
    }
}

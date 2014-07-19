using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.ServerSystem.GameHandlers
{
    public class Inventory
    {
        /// <summary>
        /// All items in this inventory. Nulls where there is an empty slot.
        /// </summary>
        public Item[] Items;

        public Inventory (int slots)
        {
            Items = new Item[slots];
        }

        /// <summary>
        /// Gets the item in a slot, or null if none.
        /// </summary>
        /// <param name="slot">The slot number</param>
        /// <returns>The item in said slot</returns>
        public Item Get(int slot)
        {
            if (slot >= Items.Length || slot < 0)
            {
                return null;
            }
            return Items[slot];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Items;

namespace mcmtestOpenTK.ServerSystem.GameHandlers
{
    public class ItemRegistry
    {
        /// <summary>
        /// All items registered.
        /// </summary>
        public Dictionary<string, Item> Items = new Dictionary<string, Item>();

        /// <summary>
        /// Gets the item corresponding to an item name.
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <returns>A valid item, or null if none</returns>
        public Item GetItemFor(string name)
        {
            Item item;
            if (Items.TryGetValue(name.ToLower(), out item))
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// Registers a new item type.
        /// </summary>
        /// <param name="item">The item type to register</param>
        public void Register(Item item)
        {
            Items.Add(item.Name.ToLower(), item);
        }

        /// <summary>
        /// Prepares the system, registering all known item types.
        /// </summary>
        public void Init()
        {
            Register(new ItemSword());
            Register(new ItemIronSword());
            Register(new ItemApple());
        }
    }
}

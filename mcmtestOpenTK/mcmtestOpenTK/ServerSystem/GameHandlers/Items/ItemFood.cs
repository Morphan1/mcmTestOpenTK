using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.Util;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Items
{
    public class ItemFood: Item
    {
        /// <summary>
        /// How much health this food can restore.
        /// </summary>
        public float HealthValue;

        /// <summary>
        /// The chance that this food went bad.
        /// </summary>
        public float PoisonChance;

        public ItemFood(string name)
            : base(name)
        {
            Description = "Edible!";
            HealthValue = 5;
            PoisonChance = 0.01f;
        }

        /// <summary>
        /// Determines whether this food is poisioned.
        /// </summary>
        public bool RollPoisoned()
        {
            return Utilities.random.NextDouble() < PoisonChance;
        }

        public override Item Duplicate(Item it = null)
        {
            ItemFood item = it == null ? (ItemFood)it : new ItemFood(Name);
            base.Duplicate(item);
            item.HealthValue = HealthValue;
            item.PoisonChance = PoisonChance;
            return item;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Items
{
    public class ItemApple: ItemFood
    {
        public ItemApple()
            : base("apple")
        {
            Texture = "items/food/apple";
            Shader = null;
            DisplayName = "Common Apple";
            CanThrow = true;
            Description = "A common apple. Restores 10 health.";
            Inheritance.Add(this);
            Weight = 10;
            Volume = 10;
            HealthValue = 10;
            PoisonChance = 0;
        }
    }
}

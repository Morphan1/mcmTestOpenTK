using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Items
{
    public class ItemIronSword: ItemSword
    {
        public ItemIronSword()
            : base()
        {
            Texture = "items/weapons/iron_sword";
            DisplayName = "Iron Sword";
            Description = "A common iron sword, swings for 8-10 damage.";
            Inheritance.Add(this);
            Weight = 11;
            Damage = 8f;
            LuckyDamage = 2f;
            Luck = 0.5f;
        }
    }
}

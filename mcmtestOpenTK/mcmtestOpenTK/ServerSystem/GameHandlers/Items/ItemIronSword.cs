using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Items
{
    public class ItemIronSword: ItemSword
    {
        public ItemIronSword(string name = "iron_sword")
            : base(name)
        {
            Texture = "items/weapons/iron_sword";
            DisplayName = "Iron Sword";
            Model = "items/weapons/common_sword";
            Description = "A common iron sword, swings for 8-10 damage.";
            Inheritance.Add(this);
            Weight = 11;
            Damage = 8f;
            LuckyDamage = 2f;
            Luck = 0.5f;
        }
    }
}

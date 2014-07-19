using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Items
{
    public class ItemSword: ItemWeapon
    {
        public ItemSword(): base("sword")
        {
            Texture = "items/weapons/iron_sword";
            Shader = null;
            DisplayName = "Common Sword";
            CanThrow = true;
            Description = "A common sword, made of common iron.";
            Inheritance.Add(this);
            Weight = 10;
            Volume = 10;
            Damage = 5f;
            LuckyDamage = 1f;
            Luck = 0.5f;
        }

        public override void OnUse(Player player)
        {
            // TODO: Block
        }

        public override void OnClick(Player player)
        {
            // TODO: Swing
        }
    }
}

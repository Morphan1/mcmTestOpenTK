using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.Util;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Items
{
    public class ItemWeapon: Item
    {
        public ItemWeapon(string name)
            : base(name)
        {
            Damage = 1f;
            LuckyDamage = 2f;
            Luck = 0.5f;
            Description = "Dangerous!";
        }

        /// <summary>
        /// How much damage this weapon does.
        /// </summary>
        public float Damage;

        /// <summary>
        /// How much higher the damage can be if the attacker is lucky.
        /// </summary>
        public float LuckyDamage;

        /// <summary>
        /// How much luck the average user has.
        /// 0 = never lucky, 1 = always lucky.
        /// </summary>
        public float Luck;

        /// <summary>
        /// Calculates the damage a swing will have, with the given user luck.
        /// </summary>
        /// <param name="UserLuck">How lucky the user is</param>
        /// <returns>The damage amount</returns>
        float CalculateDamage(float UserLuck)
        {
            return Damage + LuckyDamage * Roll(Luck * UserLuck);
        }

        /// <summary>
        /// Choose a random number between 0 and 1, with a luck factor.
        /// The calculation for this is arbitrary and probably not very high quality.
        /// </summary>
        /// <param name="UserLuck">How lucky the user is</param>
        /// <returns>The luck of the roll</returns>
        public static float Roll(float UserLuck)
        {
            return (float)Math.Max(Utilities.random.NextDouble() + Utilities.random.NextDouble() * UserLuck, 1f);
        }

        public override Item Duplicate(Item it = null)
        {
            ItemWeapon item = it == null ? (ItemWeapon)it : new ItemWeapon(Name);
            base.Duplicate(item);
            item.Damage = Damage;
            item.LuckyDamage = LuckyDamage;
            item.Luck = Luck;
            return item;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class Breakable: Entity
    {
        /// <summary>
        /// The precise health number of the breakable entity.
        /// </summary>
        public double Health = 1;
        /// <summary>
        /// The maximum health number for this breakable entity.
        /// </summary>
        public double MaxHealth = 1;
        /// <summary>
        /// How fast this breakable entity should naturally heal.
        /// In HP / second.
        /// </summary>
        public double HealRate = 0;
        /// <summary>
        /// How fast this breakable entity should naturally take damage.
        /// In HP / second.
        /// </summary>
        public double HurtRate = 0;
        /// <summary>
        /// Whether the breakable entity is still considered to be alive and intact.
        /// </summary>
        public bool IsAlive = true;

        /// <summary>
        /// Ticks the breakable entity, including health calculations.
        /// </summary>
        public override void Update()
        {
            if (IsAlive)
            {
                if (HealRate > 0 && Health < MaxHealth)
                {
                    Health += HealRate * MainGame.Delta;
                    if (Health > MaxHealth)
                    {
                        Health = MaxHealth;
                    }
                }
                if (HurtRate > 0)
                {
                    Damage(HurtRate * MainGame.Delta);
                }
            }
            base.Update();
        }

        /// <summary>
        /// Does damage to the breakable entity.
        /// </summary>
        /// <param name="damage">How much damage to do.</param>
        public void Damage(double damage)
        {
            if (damage >= Health)
            {
                Health = 0;
                IsAlive = false;
            }
            else
            {
                Health -= damage;
            }
        }
    }
}

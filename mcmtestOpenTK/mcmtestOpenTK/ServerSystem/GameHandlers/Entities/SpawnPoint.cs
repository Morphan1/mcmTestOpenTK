using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    public class SpawnPoint: Entity
    {
        public SpawnPoint(): base(false, false, EntityType.SPAWNPOINT)
        {
        }

        public override byte[] GetData()
        {
            return new byte[0];
        }

        public override List<Variable> GetSaveVars()
        {
            return base.GetSaveVars();
        }

        public override void Kill()
        {
        }

        /// <summary>
        /// Returns whether the spawn point is blocked (and shouldn't be used for spawning).
        /// </summary>
        /// <returns>Whether this is blocked</returns>
        public bool IsBlocked()
        {
            return Collision.Box(Position, Player.DefaultMins, Player.DefaultMaxes);
        }

        public override bool HandleVariable(string varname, string vardata)
        {
            return base.HandleVariable(varname, vardata);
        }

        /// <summary>
        /// Do not call: This entity does not tick!
        /// </summary>
        public override void Tick()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.GameHelpers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    public abstract class Entity
    {
        /// <summary>
        /// Run every time the world ticks.
        /// </summary>
        public abstract void Tick();

        /// <summary>
        /// Run when the entity is being removed.
        /// </summary>
        public abstract void Kill();

        /// <summary>
        /// Return binary network data specifying this entity's custom settings.
        /// </summary>
        public abstract byte[] GetData();

        /// <summary>
        /// Handle variable strings from the map file.
        /// </summary>
        /// <param name="varname">The variable name</param>
        /// <param name="vardata">The variable's data</param>
        /// <returns>Whether the variable was valid</returns>
        public abstract bool HandleVariable(string varname, string vardata);

        /// <summary>
        /// Whether this entity needs to be ticked.
        /// </summary>
        public readonly bool TickMe;

        /// <summary>
        /// Where the entity is at in the world.
        /// </summary>
        public Location Position = Location.Zero;

        /// <summary>
        /// What world the entity is in.
        /// </summary>
        public World world;

        /// <summary>
        /// Whether the entity is still in the world.
        /// </summary>
        public bool Valid = false;

        /// <summary>
        /// The world-unique ID of the entity, assigned at spawn time.
        /// </summary>
        public ulong UniqueID;

        /// <summary>
        /// What type of entity this is.
        /// </summary>
        public EntityType Type = EntityType.NUL;

        public Entity(bool _TickMe, EntityType _type)
        {
            TickMe = _TickMe;
            Type = _type;
        }
    }
}

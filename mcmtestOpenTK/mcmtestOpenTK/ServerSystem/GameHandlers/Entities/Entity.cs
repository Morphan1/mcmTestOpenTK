using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

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
        /// This method should invert GetSaveVars.
        /// </summary>
        /// <param name="varname">The variable name</param>
        /// <param name="vardata">The variable's data</param>
        /// <returns>Whether the variable was valid</returns>
        public virtual bool HandleVariable(string varname, string vardata)
        {
            if (varname == "position")
            {
                Position = Location.FromString(vardata);
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets a list of all variables on this entity, for saving maps.
        /// This method should invert HandleVariable.
        /// </summary>
        /// <returns>A full list of saved variables</returns>
        public virtual List<Variable> GetSaveVars()
        {
            List<Variable> ToReturn = new List<Variable>();
            ToReturn.Add(new Variable("position", Position.ToSimpleString()));
            return ToReturn;
        }

        /// <summary>
        /// Get the first collision of a box line.
        /// </summary>
        /// <param name="mins">The mins of the line</param>
        /// <param name="maxs">The maxs of the line</param>
        /// <param name="start">The start of the line</param>
        /// <param name="end">The end of the line</param>
        /// <param name="normal">The normal of the hit, or NaN if none</param>
        /// <returns>The location of the hit, or NaN if none</returns>
        public virtual Location ClosestBox(Location mins, Location maxs, Location start, Location end, out Location normal)
        {
            normal = Location.NaN;
            return Location.NaN;
        }

        /// <summary>
        /// Checks if a point is contained inside the entity.
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>Whether it is contained</returns>
        public virtual bool Point(Location point)
        {
            return false;
        }

        /// <summary>
        /// Checks whether a box intersects the entity.
        /// </summary>
        /// <param name="mins">The mins of the box</param>
        /// <param name="maxs">The maxs of the box</param>
        /// <returns>Whether it intersects
        public virtual bool Box(Location mins, Location maxs)
        {
            return false;
        }

        /// <summary>
        /// Whether this entity needs to be ticked.
        /// </summary>
        public readonly bool TickMe;

        /// <summary>
        /// Whether this entity should be sent to the client.
        /// </summary>
        public readonly bool NetTransmit;

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
        /// Whether this entity is solid (for collision).
        /// </summary>
        public bool Solid = false;

        /// <summary>
        /// How far below the origin location the collision box goes.
        /// </summary>
        public Location Mins = new Location(-1);

        /// <summary>
        /// How far past the origin location the collision box goes.
        /// </summary>
        public Location Maxs = new Location(1);

        /// <summary>
        /// The world-unique ID of the entity, assigned at spawn time.
        /// </summary>
        public ulong UniqueID;

        /// <summary>
        /// Called when the entity has finished loading variables.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// What type of entity this is.
        /// </summary>
        public EntityType Type = EntityType.NUL;

        public Entity(bool _TickMe, bool _NetTransmit, EntityType _type)
        {
            TickMe = _TickMe;
            NetTransmit = _NetTransmit;
            Type = _type;
        }
    }
}

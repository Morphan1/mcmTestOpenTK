using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public abstract class Entity: Renderable
    {
        /// <summary>
        /// The precise X/Y/Z location of the entity.
        /// </summary>
        public Location Position = Location.Zero;

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
        public Location Maxs = Location.One;

        /// <summary>
        /// A fairly unique ID stored as long as the entity is alive.
        /// </summary>
        public ulong UniqueID;

        public Entity(bool _TickMe, EntityType _type)
        {
            TickMe = _TickMe;
            Type = _type;
        }

        /// <summary>
        /// Ticks the entity, including running of basic movement and related handlers.
        /// </summary>
        public abstract void Tick();

        /// <summary>
        /// Reads binary packet data for the entity.
        /// </summary>
        /// <param name="data">Data from the network</param>
        public abstract void ReadBytes(byte[] data);

        /*
        public virtual Location Closest(Location start, Location target)
        {
            return Location.NaN;
        }
        */
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
        /// <returns>Whether it intersects</returns>
        public virtual bool Box(Location mins, Location maxs)
        {
            return false;
        }

        /// <summary>
        /// Whether the entity should tick regularly.
        /// </summary>
        public readonly bool TickMe;

        /// <summary>
        /// Whether the entity is still in the world.
        /// </summary>
        public bool IsValid = false;

        /// <summary>
        /// Whether the entity should be prevented from spawning.
        /// </summary>
        public bool IsCorrupt = false;

        /// <summary>
        /// What type of entity this is.
        /// </summary>
        public readonly EntityType Type;

        public static Entity FromType(EntityType type)
        {
            switch (type)
            {
                case EntityType.PLAYER:
                    return new OtherPlayer();
                case EntityType.CUBE:
                    return new CubeEntity();
                case EntityType.BULLET:
                    return new Bullet();
                case EntityType.POLYPLANAR:
                    return new PolyPlanarEntity();
                default:
                    return null;
            }
        }
    }
}

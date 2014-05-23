using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
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
        public Vector3 Position = Vector3.Zero;

        /// <summary>
        /// Whether this entity is solid (for collision).
        /// </summary>
        public bool Solid = true;

        /// <summary>
        /// How far below the origin location the collision box goes.
        /// </summary>
        public Vector3 Mins = new Vector3(-1);

        /// <summary>
        /// How far past the origin location the collision box goes.
        /// </summary>
        public Vector3 Maxs = Vector3.One;

        /// <summary>
        /// A fairly unique ID stored as long as the entity is alive.
        /// </summary>
        public ulong UniqueID;

        public Entity(bool _TickMe)
        {
            TickMe = _TickMe;
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
        public EntityType Type = EntityType.NUL;

        public static Entity FromType(EntityType type)
        {
            switch (type)
            {
                case EntityType.PLAYER:
                    return new OtherPlayer();
                case EntityType.CUBE:
                    return new CubeEntity();
                default:
                    return null;
            }
        }
    }
}

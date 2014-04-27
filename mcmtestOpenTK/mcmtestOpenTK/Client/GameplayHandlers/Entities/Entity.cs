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
        /// Whether the entity should tick regularly.
        /// </summary>
        public readonly bool TickMe;

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
                default:
                    return null;
            }
        }
    }
}

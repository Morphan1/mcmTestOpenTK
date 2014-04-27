using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.GameHelpers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    public abstract class MovingEntity: Entity
    {
        public MovingEntity(EntityType type): base(true, type)
        {
        }

        /// <summary>
        /// What direction this entity is facing.
        /// </summary>
        public Location Direction = Location.Zero;

        /// <summary>
        /// The movement velocity of this entity.
        /// </summary>
        public Location Velocity = Location.Zero;

        public override void Tick()
        {
            Position += Velocity * Server.DeltaF;
            if (lastloc != Position)
            {
                PositionPacketOut pack = new PositionPacketOut(this, Position);
                for (int i = 0; i < world.Players.Count; i++)
                {
                    world.Players[i].Send(pack);
                }
                lastloc = Position;
            }
        }

        Location lastloc;
    }
}

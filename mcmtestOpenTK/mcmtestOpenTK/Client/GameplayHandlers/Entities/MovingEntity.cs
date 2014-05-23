using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public abstract class MovingEntity: Entity
    {
        public MovingEntity(): base(true)
        {
        }

        /// <summary>
        /// The precise X/Y/Z worldly movement speed.
        /// </summary>
        public Vector3 Velocity = Vector3.Zero;

        /// <summary>
        /// The precise Yaw/Pitch/Roll direction of the entity.
        /// </summary>
        public Vector3 Direction = Vector3.Zero;

        public override void Tick()
        {
            Vector3 target = Position + Velocity * MainGame.DeltaF;
            if (Solid)
            {
                Position = Collision.MoveForward(Position, target);
            }
            else
            {
                Position = target;
            }
        }
    }
}

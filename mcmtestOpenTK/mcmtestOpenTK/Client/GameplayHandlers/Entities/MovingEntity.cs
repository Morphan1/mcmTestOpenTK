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

        /// <summary>
        /// How fast gravity should pull the entity downward.
        /// </summary>
        public float Gravity = 0;

        /// <summary>
        /// Whether this entity should check for collision while moving.
        /// </summary>
        public bool CheckCollision = false;

        public override void Tick()
        {
            Velocity.Z -= Gravity * MainGame.DeltaF;
            Vector3 target = Position + Velocity * MainGame.DeltaF;
            if (CheckCollision)
            {
                Position = Collision.MoveForward(Position, target, Mins, Maxs);
            }
            else
            {
                Position = target;
            }
        }
    }
}

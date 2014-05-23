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
        /// How far below the origin location the collision box goes.
        /// </summary>
        public Vector3 Mins = new Vector3(-1);

        /// <summary>
        /// How far past the origin location the collision box goes.
        /// </summary>
        public Vector3 Maxs = Vector3.One;

        /// <summary>
        /// The precise Yaw/Pitch/Roll direction of the entity.
        /// </summary>
        public Vector3 Direction = Vector3.Zero;

        public override void Tick()
        {
            Position += Velocity * MainGame.DeltaF;
        }
    }
}

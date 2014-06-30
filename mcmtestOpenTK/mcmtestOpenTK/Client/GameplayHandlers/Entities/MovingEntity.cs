using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared;

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
        public Location Velocity = Location.Zero;

        /// <summary>
        /// The precise Yaw/Pitch/Roll direction of the entity.
        /// </summary>
        public Location Direction = Location.Zero;

        /// <summary>
        /// How fast gravity should pull the entity downward.
        /// </summary>
        public float Gravity = 0;

        /// <summary>
        /// Whether this entity should check for collision while moving.
        /// </summary>
        public bool CheckCollision = false;

        /// <summary>
        /// How this entity moves.
        /// </summary>
        public MovementType MoveType = MovementType.LineBox;

        // TODO: Rotation velocity

        public override void Tick()
        {
            Location oldvel = Velocity;
            Velocity.Z -= Gravity * MainGame.DeltaF;
            Location target = Position + (oldvel + Velocity) * 0.5f * MainGame.DeltaF;
            if (CheckCollision)
            {
                bool WasSolid = Solid;
                Solid = false;
                switch (MoveType)
                {
                    case MovementType.LineBox:
                        Location hitnormal;
                        Position = Collision.LineBox(Position, target, Mins, Maxs, out hitnormal);
                        break;
                    case MovementType.SlideBox:
                        Position = Collision.SlideBox(Position, target, Mins, Maxs);
                        break;
                    default:
                        Position = target;
                        break;
                }
                Solid = WasSolid;
            }
            else
            {
                Position = target;
            }
        }

        public enum MovementType
        {
            SlideBox = 1,
            LineBox = 2,
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    public abstract class MovingEntity: Entity
    {
        public MovingEntity(EntityType type, bool NetTransmit): base(true, NetTransmit, type)
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
        public MovementType MoveType = MovementType.Line;

        // TODO: Rotation velocity

        int retrans = 0;

        public override void Tick()
        {
            Tick(Server.DeltaF, false);
        }

        public virtual void Tick(float MyDelta, bool IsCustom)
        {
            Velocity.Z -= Gravity * MyDelta;
            float pZ = Position.Z;
            Location target = Position + (lastvel + Velocity) * 0.5f * MyDelta;
            if (CheckCollision)
            {
                switch (MoveType)
                {
                    case MovementType.Line:
                        Position = Collision.Line(Position, target);
                        break;
                    case MovementType.LineBox:
                        Position = Collision.LineBox(Position, target, Mins, Maxs);
                        break;
                    case MovementType.Slide:
                        Position = Collision.Slide(Position, target);
                        break;
                    case MovementType.SlideBox:
                        Position = Collision.SlideBox(Position, target, Mins, Maxs);
                        break;
                    default:
                        Position = target;
                        break;
                }
                if (Position != target)
                {
                    Collide();
                }
            }
            else
            {
                Position = target;
            }
            Velocity.Z = (Position.Z - pZ) / MyDelta;
            if (!IsCustom)
            {
                retrans++;
                if (lastvel != Velocity || lastdir != Direction || (retrans == 10 && Velocity.LengthSquared() != 0))
                {
                    retrans = 0;
                    PositionPacketOut pack = new PositionPacketOut(this, Position, Velocity, Direction);
                    for (int i = 0; i < world.Players.Count; i++)
                    {
                        if (world.Players[i] != this)
                        {
                            world.Players[i].Send(pack);
                        }
                    }
                }
            }
            lastvel = Velocity;
            lastdir = Direction;
        }

        public abstract void Collide();

        Location lastvel;
        Location lastdir;

        public override bool HandleVariable(string varname, string vardata)
        {
            if (varname == "direction")
            {
                Direction = Location.FromString(vardata);
            }
            else if (varname == "velocity")
            {
                Velocity = Location.FromString(vardata);
            }
            else if (varname == "gravity")
            {
                Gravity = Utilities.StringToFloat(vardata);
            }
            else
            {
                return base.HandleVariable(varname, vardata);
            }
            return true;
        }

        public override List<Variable> GetSaveVars()
        {
            List<Variable> ToReturn = base.GetSaveVars();
            ToReturn.Add(new Variable("direction", Direction.ToSimpleString()));
            ToReturn.Add(new Variable("velocity", Velocity.ToSimpleString()));
            ToReturn.Add(new Variable("gravity", Gravity.ToString()));
            return ToReturn;
        }
    }

    public enum MovementType
    {
        Slide = 1,
        SlideBox = 2,
        Line = 3,
        LineBox = 4,
    }
}

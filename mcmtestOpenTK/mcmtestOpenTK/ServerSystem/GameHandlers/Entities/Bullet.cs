using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    class Bullet: MovingEntity
    {
        public Bullet(): base(EntityType.BULLET, true)
        {
            CheckCollision = true;
            Mins = new Location(-0.5f);
            Maxs = new Location(0.5f);
            MoveType = MovementType.LineBox;
            Solid = false;
        }

        public override void Kill()
        {
        }

        public override byte[] GetData()
        {
            byte[] toret = new byte[12 * 2];
            Direction.ToBytes().CopyTo(toret, 0);
            Velocity.ToBytes().CopyTo(toret, 12);
            return toret;
        }

        public override void Init()
        {
        }

        public override bool HandleVariable(string varname, string vardata)
        {
            return base.HandleVariable(varname, vardata);
        }

        public override List<Variable> GetSaveVars()
        {
            return base.GetSaveVars();
        }

        public override void Collide()
        {
            world.Destroy(this);
        }
    }
}

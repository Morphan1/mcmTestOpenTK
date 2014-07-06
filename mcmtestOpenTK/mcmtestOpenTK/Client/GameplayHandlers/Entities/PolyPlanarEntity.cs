using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class PolyPlanarEntity: Entity
    {
        public PolyPlanarEntity()
            : base(false, EntityType.POLYPLANAR)
        {
        }

        public override bool Box(Location mins, Location maxs)
        {
            return base.Box(mins, maxs);
        }

        public override Location ClosestBox(Location mins, Location maxs, Location start, Location end, out Location normal)
        {
            return base.ClosestBox(mins, maxs, start, end, out normal);
        }

        public override bool Point(Location point)
        {
            return base.Point(point);
        }

        public override void ReadBytes(byte[] data)
        {
            // TODO
        }

        public override void Tick()
        {
        }

        public override void Draw()
        {
            // TODO
        }
    }
}

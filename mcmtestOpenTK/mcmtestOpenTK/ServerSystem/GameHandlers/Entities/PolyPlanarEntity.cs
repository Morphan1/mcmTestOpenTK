using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    class PolyPlanarEntity: Entity
    {
        List<Plane> Planes;

        public PolyPlanarEntity(List<Plane> _planes)
            : base(false, true, EntityType.POLYPLANAR)
        {
            Planes = _planes;
        }

        public override bool Box(Shared.Location mins, Shared.Location maxs)
        {
            return base.Box(mins, maxs);
        }

        public override Location ClosestBox(Location mins, Location maxs, Location start, Location end, out Location normal)
        {
            return base.ClosestBox(mins, maxs, start, end, out normal);
        }

        public override List<Variable> GetSaveVars()
        {
            List<Variable> vars = base.GetSaveVars();
            StringBuilder planestr = new StringBuilder(Planes.Count * 36);
            for (int i = 0; i < Planes.Count; i++)
            {
                planestr.Append(Planes[i].ToString()).Append("_");
            }
            vars.Add(new Variable("planes", planestr.ToString()));
            return vars;
        }

        public override bool HandleVariable(string varname, string vardata)
        {
            if (varname == "planes")
            {
                string[] data = vardata.Split('_');
                for (int i = 0; i < data.Length; i++)
                {
                    Planes.Add(Plane.FromString(data[i]));
                }
            }
            else
            {
                return base.HandleVariable(varname, vardata);
            }
            return true;
        }

        public override byte[] GetData()
        {
            byte[] bytes = new byte[Planes.Count * 12 * 3];
            for (int i = 0; i < Planes.Count; i++)
            {
                Planes[i].ToBytes().CopyTo(bytes, i * 36);
            }
            return bytes;
        }

        public override void Init()
        {
        }

        public override void Kill()
        {
        }

        public override bool Point(Location point)
        {
            return base.Point(point);
        }

        public override void Tick()
        {
        }
    }
}

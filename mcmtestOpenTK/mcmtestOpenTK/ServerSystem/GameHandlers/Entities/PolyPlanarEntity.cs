using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    class PolyPlanarEntity: Entity
    {
        List<Plane> Planes;
        List<string> Textures;

        public PolyPlanarEntity()
            : base(false, true, EntityType.POLYPLANAR)
        {
            Planes = new List<Plane>();
            Textures = new List<string>();
        }

        public override bool Box(Location mins, Location maxs)
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
                    if (data[i].Length > 0)
                    {
                        Plane pl = Plane.FromString(data[i]);
                        Planes.Add(pl);
                    }
                }
            }
            else if (varname == "texture")
            {
                for (int i = 0; i < 12; i++)
                {
                    Textures.Add(vardata);
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
            byte[] bytes = new byte[Planes.Count * (36 + 4)];
            for (int i = 0; i < Planes.Count; i++)
            {
                Planes[i].ToBytes().CopyTo(bytes, i * (36 + 4));
                Plane plane2 = Plane.FromBytes(Planes[i].ToBytes());
                BitConverter.GetBytes(NetStringManager.GetStringID(Textures[i])).CopyTo(bytes, (i + 1) * (36 + 4) - 4);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    public class CubeEntity: Entity
    {
        /// <summary>
        /// How big the cube is.
        /// </summary>
        public Location Scale = new Location(1);

        /// <summary>
        /// What texture to render with.
        /// </summary>
        public String texture;

        public CubeEntity(): base(false, EntityType.CUBE)
        {
            Solid = true;
            Mins = new Location(0);
        }

        /// <summary>
        /// Do not call: This entity does not tick!
        /// </summary>
        public override void Tick()
        {
        }

        public override void Kill()
        {
        }

        public override byte[] GetData()
        {
            byte[] texturebytes = FileHandler.encoding.GetBytes(texture);
            byte[] scalebytes = Scale.ToBytes();
            byte[] toret = new byte[scalebytes.Length + texturebytes.Length];
            scalebytes.CopyTo(toret, 0);
            texturebytes.CopyTo(toret, scalebytes.Length);
            return toret;
        }

        public override bool HandleVariable(string varname, string vardata)
        {
            if (varname == "scale")
            {
                Scale = Location.FromString(vardata);
                Maxs = Scale;
            }
            else if (varname == "texture")
            {
                texture = vardata;
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
            ToReturn.Add(new Variable("scale", Scale.ToSimpleString()));
            ToReturn.Add(new Variable("texture", texture));
            return ToReturn;
        }
    }
}

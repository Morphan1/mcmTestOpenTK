using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    public class CubeEntity: Entity
    {
        /// <summary>
        /// How big the cube is.
        /// </summary>
        public Location Scale = new Location(1);

        /// <summary>
        /// The horizontal scaling of the texture.
        /// </summary>
        public float Texture_HScale = 1;

        /// <summary>
        /// The vertical scaling of the texture.
        /// </summary>
        public float Texture_VScale = 1;

        /// <summary>
        /// The horizontal shift of the texture.
        /// </summary>
        public float Texture_HShift = 0;

        /// <summary>
        /// The vertical shift of the texture.
        /// </summary>
        public float Texture_VShift = 0;

        /// <summary>
        /// What texture to render with.
        /// </summary>
        public String texture;

        public CubeEntity(): base(false, true, EntityType.CUBE)
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
            byte[] scalebytes = Scale.ToBytes();
            byte[] toret = new byte[scalebytes.Length + 4 * 4 + 4];
            int pos = 0;
            scalebytes.CopyTo(toret, pos);
            pos += scalebytes.Length;
            BitConverter.GetBytes(Texture_HScale).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(Texture_VScale).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(Texture_HShift).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(Texture_VShift).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(NetStringManager.GetStringID(texture)).CopyTo(toret, pos);
            pos += 4;
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
                texture = FileHandler.CleanFileName(vardata);
            }
            else if (varname == "texture_hscale")
            {
                Texture_HScale = Utilities.StringToFloat(vardata);
            }
            else if (varname == "texture_vscale")
            {
                Texture_VScale = Utilities.StringToFloat(vardata);
            }
            else if (varname == "texture_hshift")
            {
                Texture_HShift = Utilities.StringToFloat(vardata);
            }
            else if (varname == "texture_vshift")
            {
                Texture_VShift = Utilities.StringToFloat(vardata);
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
            ToReturn.Add(new Variable("texture_hscale", Texture_HScale.ToString()));
            ToReturn.Add(new Variable("texture_vscale", Texture_VScale.ToString()));
            ToReturn.Add(new Variable("texture_hshift", Texture_HShift.ToString()));
            ToReturn.Add(new Variable("texture_vshift", Texture_VShift.ToString()));
            return ToReturn;
        }
    }
}

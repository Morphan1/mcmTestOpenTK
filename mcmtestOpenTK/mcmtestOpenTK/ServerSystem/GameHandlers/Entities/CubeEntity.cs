﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;
using mcmtestOpenTK.Shared.Game;

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
        /// The rotation of the texture.
        /// </summary>
        public float Texture_Rotation = 0;

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
            byte[] maxbytes = Maxs.ToBytes();
            byte[] minbytes = Mins.ToBytes();
            byte[] toret = new byte[maxbytes.Length + minbytes.Length + 4 * 5 + 4];
            int pos = 0;
            maxbytes.CopyTo(toret, pos);
            pos += maxbytes.Length;
            minbytes.CopyTo(toret, pos);
            pos += minbytes.Length;
            BitConverter.GetBytes(Texture_HScale).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(Texture_VScale).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(Texture_HShift).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(Texture_VShift).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(Texture_Rotation).CopyTo(toret, pos);
            pos += 4;
            BitConverter.GetBytes(NetStringManager.GetStringID(texture)).CopyTo(toret, pos);
            pos += 4;
            return toret;
        }

        public override bool HandleVariable(string varname, string vardata)
        {
            if (varname == "mins")
            {
                Mins = Location.FromString(vardata);
            }
            else if (varname == "maxs")
            {
                Scale = Location.FromString(vardata);
                Maxs = Scale;
            }
            else if (varname == "scale")
            {
                Scale = Location.FromString(vardata);
                Maxs = Scale + Position;
                Mins = Position;
                Position = Location.Zero;
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
            else if (varname == "texture_rotation")
            {
                Texture_Rotation = Utilities.StringToFloat(vardata);
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
            ToReturn.Add(new Variable("mins", Mins.ToSimpleString()));
            ToReturn.Add(new Variable("maxs", Maxs.ToSimpleString()));
            ToReturn.Add(new Variable("texture", texture));
            ToReturn.Add(new Variable("texture_hscale", Texture_HScale.ToString()));
            ToReturn.Add(new Variable("texture_vscale", Texture_VScale.ToString()));
            ToReturn.Add(new Variable("texture_hshift", Texture_HShift.ToString()));
            ToReturn.Add(new Variable("texture_vshift", Texture_VShift.ToString()));
            ToReturn.Add(new Variable("texture_rotation", Texture_Rotation.ToString()));
            /*
            Plane[] Planes = CalculateTriangles();
            StringBuilder planestr = new StringBuilder(Planes.Length * 36);
            for (int i = 0; i < Planes.Length; i++)
            {
                planestr.Append(Planes[i].ToString()).Append("_");
            }
            ToReturn.Add(new Variable("planes", planestr.ToString()));*/
            return ToReturn;
        }

        public override bool Point(Location spot)
        {
            Location lower = Position + Mins;
            Location upper = Position + Maxs;
            return lower.X <= spot.X && lower.Y <= spot.Y && lower.Z <= spot.Z &&
                upper.X >= spot.X && upper.Y >= spot.Y && upper.Z >= spot.Z;
        }

        public override bool Box(AABB Box2)
        {
            Location elow = Mins;
            Location ehigh = Maxs;
            Location Low = Box2.Mins;
            Location High = Box2.Maxs;
            return Low.X <= ehigh.X && Low.Y <= ehigh.Y && Low.Z <= ehigh.Z &&
                        High.X >= elow.X && High.Y >= elow.Y && High.Z >= elow.Z;
        }

        public override void Init()
        {
        }

        public override Location ClosestBox(AABB Box2, Location start, Location end, out Location normal)
        {
            return AABB.AABBClosestBox(Position, Mins, Maxs, Box2.Mins, Box2.Maxs, start, end, out normal);
        }
    }
}

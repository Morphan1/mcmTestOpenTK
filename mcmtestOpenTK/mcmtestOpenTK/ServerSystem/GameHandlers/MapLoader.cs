using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;

namespace mcmtestOpenTK.ServerSystem.GameHandlers
{
    public class MapLoader
    {
        /// <summary>
        /// Loads a map onto the specified world.
        /// </summary>
        /// <param name="world">The world to load it onto</param>
        /// <param name="map">The name of the map to load</param>
        /// <returns>Whether the map could be loaded</returns>
        public static bool LoadMap(World world, string map)
        {
            string mapdata;
            try
            {
                mapdata = FileHandler.ReadText("maps/" + map + ".map");
            }
            catch (UnknownFileException)
            {
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
                return false;
            }
            if (mapdata.Length == 0)
            {
                return false;
            }
            // To create an ideal world, one must first destroy all that is not ideal
            world.DestroyWorld(!world.HasMap);
            int start = 0;
            bool inBlock = false;
            string type = "";
            for (int i = 0; i < mapdata.Length; i++)
            {
                if (!inBlock && mapdata[i] == '{')
                {
                    type = mapdata.Substring(start, i - start - 1);
                    inBlock = true;
                    start = i + 1;
                }
                if (inBlock && mapdata[i] == '}')
                {
                    string data = mapdata.Substring(start, i - start - 1);
                    inBlock = false;
                    start = i + 1;
                    MakeObject(world, map, type, data);
                    type = "";
                }
            }
            world.HasMap = true;
            return true;
        }

        static void MakeObject(World world, string map, string type, string data)
        {
            type = type.Replace('\n', ' ').Replace('\t', ' ').Replace(" ", "").ToLower();
            data = data.Replace('\n', ' ').Replace('\t', ' ');
            string varname = "";
            int start = 0;
            bool quoted = false;
            bool invar = false;
            StringBuilder vardata = new StringBuilder();
            Entity ent;
            switch (type)
            {
                case "cube":
                    ent = new CubeEntity();
                    break;
                case "bullet":
                    ent = new Bullet();
                    break;
                default:
                    ErrorHandler.HandleError("Error loading map '" + map + "': invalid entity type '" + type + "'.");
                    return;
            }
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == ':' && !invar)
                {
                    varname = data.Substring(start, i - start).Replace(" ", "");
                    start = i + 1;
                    invar = true;
                }
                else if (data[i] == '"' && invar)
                {
                    quoted = !quoted;
                }
                else if (data[i] == ' ' && invar && !quoted)
                {
                }
                else if (data[i] == ';' && invar && !quoted)
                {
                    string vardatastr = vardata.ToString();
                    if (!ent.HandleVariable(varname, vardatastr))
                    {
                        ErrorHandler.HandleError("Error loading map '" + map +
                            "': invalid variable '" + vardatastr + "' for entity type '" + type + "'.");
                    }
                    varname = "";
                    invar = false;
                    vardata = new StringBuilder();
                    start = i + 1;
                }
                else if (invar)
                {
                    vardata.Append(data[i]);
                }
            }
            world.Spawn(ent);
        }

        /// <summary>
        /// Saves the world to a map file.
        /// </summary>
        /// <param name="world">The world to save</param>
        /// <param name="mapname">The name of the map to save to</param>
        public static void SaveMap(World world, string mapname)
        {
            string SaveStr = GetMapString(world);
            try
            {
                FileHandler.WriteText("maps/" + mapname + ".map", SaveStr);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }

        static string GetMapString(World world)
        {
            StringBuilder ToReturn = new StringBuilder();
            for (int i = 0; i < world.Entities.Count; i++)
            {
                Entity ent = world.Entities[i];
                string type;
                switch (ent.Type)
                {
                    case EntityType.CUBE:
                        type = "cube";
                        break;
                    case EntityType.BULLET:
                        type = "bullet";
                        break;
                    default:
                        type = null;
                        break;
                }
                if (type == null)
                {
                    continue;
                }
                ToReturn.Append(type + "\n{\n");
                foreach (Variable var in ent.GetSaveVars())
                {
                    ToReturn.Append("\t").Append(var.Name).Append(": ").Append(var.Value).Append(";\n");
                }
                ToReturn.Append("}\n");
            }
            return ToReturn.ToString();
        }
    }
}

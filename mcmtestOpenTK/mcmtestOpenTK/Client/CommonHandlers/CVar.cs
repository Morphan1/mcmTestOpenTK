using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.CommonHandlers
{
    [Flags]
    public enum CVarFlag
    {
        None = 0x0,
        ReadOnly = 0x1,
        Textual = 0x2,
        Numeric = 0x4,
        Boolean = 0x8,
        Delayed = 0x10,
        UserMade = 0x20,
    }
    public class CVar
    {
        // Text CVars
        /// <summary>
        /// Whether to only render text when needed.
        /// TODO: IMPLEMENT
        /// </summary>
        public static CVar t_fastrender;
        /// <summary>
        /// Whether to only render side-text when needed.
        /// TODO: IMPLEMENT
        /// </summary>
        public static CVar t_sidetextfastrender;
        /// <summary>
        /// Whether to allow '^k' (Obfuscated) text.
        /// TODO: IMPLEMENT
        /// </summary>
        public static CVar t_allowobfu;
        /// <summary>
        /// Whether to allow '^R' (Random) text.
        /// TODO: IMPLEMENT
        /// </summary>
        public static CVar t_allowrandom;
        /// <summary>
        /// Whether to allow '^J' (Jello) text.
        /// TODO: IMPLEMENT
        /// </summary>
        public static CVar t_allowjello;
        /// <summary>
        /// Whether to draw HD text '^e' (Emphasis) (2 pixels out instead of 1)
        /// TODO: IMPLEMENT
        /// </summary>
        public static CVar t_betteremphasis;
        /// <summary>
        /// Whether to draw HD text '^d' (Drop-Shadow) (2 pixels out instead of 1)
        /// TODO: IMPLEMENT
        /// </summary>
        public static CVar t_bettershadow;

        /// <summary>
        /// A list of all existent CVars.
        /// </summary>
        public static List<CVar> CVars;

        /// <summary>
        /// Prepares the CVar system, generating default CVars.
        /// </summary>
        public static void Init()
        {
            CVars = new List<CVar>();
            // Text CVars
            t_fastrender = Register("t_fastrender", "false", CVarFlag.Boolean);
            t_sidetextfastrender = Register("t_sidetextfastrender", "false", CVarFlag.Boolean);
            t_allowobfu = Register("t_allowobfu", "true", CVarFlag.Boolean);
            t_allowrandom = Register("t_allowrandom", "true", CVarFlag.Boolean);
            t_allowjello = Register("t_allowjello", "true", CVarFlag.Boolean);
            t_betteremphasis = Register("t_betteremphasis", "true", CVarFlag.Boolean);
            t_bettershadow = Register("t_bettershadow", "true", CVarFlag.Boolean);
            // TODO: Other CVars
            // TODO: system file path + OpenGL info + OS name/version + RAM + harddrive space
            // TODO: other system info
        }

        /// <summary>
        /// Registers a new CVar.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The default value</param>
        /// <returns>The registered CVar</returns>
        public static CVar Register(string CVar, string value, CVarFlag flags)
        {
            CVar cvar = new CVar(CVar.ToLower(), value, flags);
            CVars.Add(cvar);
            return cvar;
        }

        /// <summary>
        /// Sets the value of an existing CVar, or generates a new one.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The value to set it to</param>
        /// <returns>The set CVar</returns>
        public static CVar AbsoluteSet(string CVar, string value)
        {
            CVar gotten = Get(CVar);
            if (gotten == null)
            {
                gotten = Register(CVar, value, CVarFlag.UserMade);
            }
            else
            {
                gotten.Set(value);
            }
            return gotten;
        }

        /// <summary>
        /// Gets an existing CVar, or generates a new one with a specific default value.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The default value if it doesn't exist</param>
        /// <returns>The found CVar</returns>
        public static CVar AbsoluteGet(string CVar, string value)
        {
            CVar gotten = Get(CVar);
            if (gotten == null)
            {
                gotten = Register(CVar, value, CVarFlag.UserMade);
            }
            return gotten;
        }

        /// <summary>
        /// Gets the CVar that matches a specified name.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <returns>The found CVar, or null if none</returns>
        public static CVar Get(string CVar)
        {
            string cvlow = CVar.ToLower();
            for (int i = 0; i < CVars.Count; i++)
            {
                if (CVars[i].Name == cvlow)
                {
                    return CVars[i];
                }
            }
            return null;
        }

        /// <summary>
        /// The name of the CVar.
        /// </summary>
        public string Name;

        /// <summary>
        /// The value of the CVar, as text.
        /// </summary>
        public string Value;

        /// <summary>
        /// The value of the CVar, as an int.
        /// </summary>
        public int ValueI;

        /// <summary>
        /// The value of the CVar, as a float.
        /// </summary>
        public float ValueF;

        /// <summary>
        /// The value of the CVar, as a boolean.
        /// </summary>
        public bool ValueB;

        /// <summary>
        /// The CVar flags set.
        /// </summary>
        public CVarFlag Flags;

        public CVar(string newname, string newvalue, CVarFlag newflags)
        {
            Name = newname;
            Set(newvalue);
            Flags = newflags;
        }

        /// <summary>
        /// Sets the CVar to a new value.
        /// </summary>
        /// <param name="newvalue">The value to set the CVar to</param>
        public void Set(string newvalue)
        {
            Value = newvalue;
            ValueI = Utilities.StringToInt(newvalue);
            ValueF = Utilities.StringToFloat(newvalue);
            ValueB = newvalue.ToLower() == "true" || ValueF > 0f;
        }

        /// <summary>
        /// Returns a human-readable list of flags.
        /// </summary>
        /// <returns>The flag list</returns>
        public string FlagInfo()
        {
            if (Flags == CVarFlag.None)
            {
                return "None";
            }
            string Type = null;
            if (Flags.HasFlag(CVarFlag.Boolean))
            {
                Type = "Boolean";
            }
            else if (Flags.HasFlag(CVarFlag.Textual))
            {
                Type = "Textual";
            }
            else if (Flags.HasFlag(CVarFlag.Numeric))
            {
                Type = "Numeric";
            }
            else if (Flags.HasFlag(CVarFlag.UserMade))
            {
                Type = "User-Made";
            }
            if (Flags.HasFlag(CVarFlag.ReadOnly))
            {
                if (Type != null)
                {
                    return "ReadOnly, " + Type;
                }
                else
                {
                    return "ReadOnly";
                }
            }
            else if (Flags.HasFlag(CVarFlag.Delayed))
            {
                if (Type != null)
                {
                    return "Delayed, " + Type;
                }
                else
                {
                    return "Delayed";
                }
            }
            else
            {
                if (Type != null)
                {
                    return Type;
                }
                else
                {
                    return "???UNKNOWN-FLAGS???";
                }
            }
        }

        /// <summary>
        /// Returns a human-readable colored information line from this CVar.
        /// </summary>
        /// <returns>The information</returns>
        public string Info()
        {
            return TextStyle.Color_Simple + "Name: '" + TextStyle.Color_Separate + Name + TextStyle.Color_Simple + "', value: '" + 
                TextStyle.Color_Separate + Value + TextStyle.Color_Simple + "', numeric value: " + TextStyle.Color_Separate +
                ValueF + TextStyle.Color_Simple + ", boolean value: " + TextStyle.Color_Separate + (ValueB ? "true": "false") +
                TextStyle.Color_Simple + ", flags: " + TextStyle.Color_Separate + FlagInfo();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using Microsoft.VisualBasic.Devices;
using mcmtestOpenTK.Client.GlobalHandler;

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
        public static CVar t_fastrender, t_sidetextfastrender, t_allowobfu, t_allowrandom, t_allowjello, t_betteremphasis, t_bettershadow;

        //Graphics CVars
        public static CVar g_vsync, g_fov, g_screenwidth, g_screenheight, g_fullscreen;

        // System CVars
        public static CVar s_filepath, s_osversion, s_user, s_dotnetversion, s_totalram, s_culture, s_processors, s_machinename;

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
            // TODO: IMPLEMENT BELOW CVAR
            t_fastrender = Register("t_fastrender", "false", CVarFlag.Boolean); // Whether to only render text when needed.
            // TODO: IMPLEMENT BELOW CVAR
            t_sidetextfastrender = Register("t_sidetextfastrender", "false", CVarFlag.Boolean); // Whether to only render side-text when needed.
            t_allowobfu = Register("t_allowobfu", "true", CVarFlag.Boolean); // Whether to allow '^k' (Obfuscated) text.
            t_allowrandom = Register("t_allowrandom", "true", CVarFlag.Boolean); // Whether to allow '^R' (Random) text.
            t_allowjello = Register("t_allowjello", "true", CVarFlag.Boolean); // Whether to allow '^J' (Jello) text.
            t_betteremphasis = Register("t_betteremphasis", "true", CVarFlag.Boolean); // Whether to draw HD text '^e' (Emphasis) (2 pixels out instead of 1)
            t_bettershadow = Register("t_bettershadow", "true", CVarFlag.Boolean); // Whether to draw HD text '^d' (Drop-Shadow) (2 pixels out instead of 1)
            // Graphics CVars
            g_vsync = Register("g_vsync", "0", CVarFlag.Numeric | CVarFlag.Delayed); // What VSync mode to use. 0 = Off, 1 = On, 2 = Adaptive.
            g_fov = Register("g_fov", "45", CVarFlag.Numeric); // What field-of-vision range to use.
            g_screenwidth = Register("g_screenwidth", MainGame.ScreenWidth.ToString(), CVarFlag.Numeric | CVarFlag.Delayed); // The X-width (size) of the window on-screen.
            g_screenheight = Register("g_screenheight", MainGame.ScreenHeight.ToString(), CVarFlag.Numeric | CVarFlag.Delayed); // The Y-height (size) of the window on-screen.
            // TODO: IMPLEMENT BELOW CVAR
            g_fullscreen = Register("g_fullscreen", "false", CVarFlag.Boolean | CVarFlag.Delayed); // Whether to make the render window occupy the entire screen.
            // TODO: More graphics CVars
            // System CVars
            ComputerInfo CI = new ComputerInfo();
            s_filepath = Register("s_filepath", FileHandler.BaseDirectory, CVarFlag.Textual | CVarFlag.ReadOnly); // The current system environment filepath (The directory of /data).
            s_osversion = Register("s_osversion", Environment.OSVersion.VersionString, CVarFlag.Textual | CVarFlag.ReadOnly); // The name and version of the operating system the game is being run on.
            s_user = Register("s_user", Environment.UserName, CVarFlag.Textual | CVarFlag.ReadOnly); // The name of the system user running the game.
            s_dotnetversion = Register("s_dotnetversion", Environment.Version.ToString(), CVarFlag.Textual | CVarFlag.ReadOnly); // The system's .NET (CLR) version string.
            s_totalram = Register("s_totalram", CI.TotalPhysicalMemory.ToString(), CVarFlag.Numeric | CVarFlag.ReadOnly); // How much RAM the system has.
            s_culture = Register("s_culture", System.Globalization.CultureInfo.CurrentUICulture.EnglishName, CVarFlag.Textual | CVarFlag.ReadOnly); // The system culture (locale).
            s_processors = Register("s_processors", Environment.ProcessorCount.ToString(), CVarFlag.Numeric | CVarFlag.ReadOnly); // The number of processors the system has.
            s_machinename = Register("s_machinename", Environment.MachineName, CVarFlag.Textual | CVarFlag.ReadOnly); // The name given to the computer.
            // TODO: OpenGL info
            // TODO: other system info
            // TODO: Other CVars
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
        /// The value of the CVar, as a long.
        /// </summary>
        public long ValueL;

        /// <summary>
        /// The value of the CVar, as an int.
        /// </summary>
        public int ValueI;

        /// <summary>
        /// The value of the CVar, as a double.
        /// </summary>
        public double ValueD;

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
            if (Flags.HasFlag(CVarFlag.ReadOnly))
            {
                return;
            }
            Value = newvalue;
            ValueL = Utilities.StringToLong(newvalue);
            ValueI = (int)ValueL;
            ValueD = Utilities.StringToDouble(newvalue);
            ValueF = (float)ValueD;
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
                ValueD + TextStyle.Color_Simple + ", boolean value: " + TextStyle.Color_Separate + (ValueB ? "true": "false") +
                TextStyle.Color_Simple + ", flags: " + TextStyle.Color_Separate + FlagInfo();
        }
    }
}

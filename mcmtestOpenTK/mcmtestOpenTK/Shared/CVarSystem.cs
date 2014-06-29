using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Shared
{
    public class CVarSystem
    {
        /// <summary>
        /// A list of all existent CVars.
        /// </summary>
        public List<CVar> CVars;

        /// <summary>
        /// The client/server outputter to use.
        /// </summary>
        public Outputter Output;

        // System CVars
        public static CVar s_filepath, s_osversion, s_user, s_dotnetversion, s_totalram, s_culture, s_processors, s_machinename;

        public CVarSystem(Outputter _output)
        {
            CVars = new List<CVar>();
            Output = _output;
            Output.CVarSys = this;

            // System CVars
            s_filepath = Register("s_filepath", FileHandler.BaseDirectory, CVarFlag.Textual | CVarFlag.ReadOnly); // The current system environment filepath (The directory of /data).
            s_osversion = Register("s_osversion", Environment.OSVersion.VersionString, CVarFlag.Textual | CVarFlag.ReadOnly); // The name and version of the operating system the game is being run on.
            s_user = Register("s_user", Environment.UserName, CVarFlag.Textual | CVarFlag.ReadOnly); // The name of the system user running the game.
            s_dotnetversion = Register("s_dotnetversion", Environment.Version.ToString(), CVarFlag.Textual | CVarFlag.ReadOnly); // The system's .NET (CLR) version string.
#if WINDOWS
            s_totalram = Register("s_totalram", new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory.ToString(), CVarFlag.Numeric | CVarFlag.ReadOnly); // How much RAM the system has.
#endif
            s_culture = Register("s_culture", System.Globalization.CultureInfo.CurrentUICulture.EnglishName, CVarFlag.Textual | CVarFlag.ReadOnly); // The system culture (locale).
            s_processors = Register("s_processors", Environment.ProcessorCount.ToString(), CVarFlag.Numeric | CVarFlag.ReadOnly); // The number of processors the system has.
            s_machinename = Register("s_machinename", Environment.MachineName, CVarFlag.Textual | CVarFlag.ReadOnly); // The name given to the computer.
            // TODO: other system info
        }

        /// <summary>
        /// Registers a new CVar.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The default value</param>
        /// <returns>The registered CVar</returns>
        public CVar Register(string CVar, string value, CVarFlag flags)
        {
            CVar cvar = new CVar(CVar.ToLower(), value, flags, this);
            CVars.Add(cvar);
            return cvar;
        }

        /// <summary>
        /// Sets the value of an existing CVar, or generates a new one.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The value to set it to</param>
        /// <param name="force">Whether to force a server send</param>
        /// <returns>The set CVar</returns>
        public CVar AbsoluteSet(string CVar, string value, bool force = false)
        {
            CVar gotten = Get(CVar);
            if (gotten == null)
            {
                gotten = Register(CVar, value, CVarFlag.UserMade);
            }
            else
            {
                gotten.Set(value, force);
            }
            return gotten;
        }

        /// <summary>
        /// Gets an existing CVar, or generates a new one with a specific default value.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The default value if it doesn't exist</param>
        /// <returns>The found CVar</returns>
        public CVar AbsoluteGet(string CVar, string value)
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
        public CVar Get(string CVar)
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
    }
}

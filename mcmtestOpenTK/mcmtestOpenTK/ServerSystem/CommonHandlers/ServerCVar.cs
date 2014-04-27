using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using Microsoft.VisualBasic.Devices;

namespace mcmtestOpenTK.ServerSystem.CommonHandlers
{
    class ServerCVar
    {
        /// <summary>
        /// The CVar System the server will use.
        /// </summary>
        public static CVarSystem system;

        // Game CVars
        public static CVar g_fps, g_online;

        // System CVars
        public static CVar s_filepath, s_osversion, s_user, s_dotnetversion, s_totalram, s_culture, s_processors, s_machinename;

        /// <summary>
        /// Prepares the CVar system, generating default CVars.
        /// </summary>
        public static void Init()
        {
            system = new CVarSystem();
            // Game CVars
            g_fps = Register("g_fps", "20", CVarFlag.Numeric); // The target frames-per-second the server will run at.
            g_online = Register("g_online", "true", CVarFlag.Boolean); // Whether the server should require all users log in through the global server.
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
            // TODO: other system info
            // TODO: Other CVars
        }

        static CVar Register(string name, string value, CVarFlag flags)
        {
            return system.Register(name, value, flags);
        }
    }
}

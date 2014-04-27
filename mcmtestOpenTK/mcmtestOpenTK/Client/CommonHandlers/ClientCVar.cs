﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using Microsoft.VisualBasic.Devices;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommonHandlers
{
    public class ClientCVar
    {
        /// <summary>
        /// The CVar System the client will use.
        /// </summary>
        public static CVarSystem system;

        // Text CVars
        public static CVar t_fastrender, t_sidetextfastrender, t_allowobfu, t_allowrandom, t_allowjello, t_betteremphasis, t_bettershadow;

        //Graphics CVars
        public static CVar g_vsync, g_fov, g_screenwidth, g_screenheight, g_fullscreen;

        // System CVars
        public static CVar s_filepath, s_osversion, s_user, s_dotnetversion, s_totalram, s_culture, s_processors, s_machinename;

        /// <summary>
        /// Prepares the CVar system, generating default CVars.
        /// </summary>
        public static void Init(Outputter output)
        {
            system = new CVarSystem(output);

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

        static CVar Register(string name, string value, CVarFlag flags)
        {
            return system.Register(name, value, flags);
        }
    }
}

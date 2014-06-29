using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;

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

        // SerVer CVars
        public static CVar v_name;

        // Network CVars
        public static CVar n_port;

        /// <summary>
        /// Prepares the CVar system, generating default CVars.
        /// </summary>
        public static void Init(Outputter output)
        {
            system = new CVarSystem(output);
            // Game CVars
            g_fps = Register("g_fps", "20", CVarFlag.Numeric); // The target frames-per-second the server will run at.
            g_online = Register("g_online", "true", CVarFlag.Boolean); // Whether the server should require all users log in through the global server.
            // SerVer CVars
            v_name = Register("v_name", "My New Server", CVarFlag.Textual); // The name of the server
            // Network CVars
            n_port = Register("n_port", "26805", CVarFlag.Numeric | CVarFlag.InitOnly); // What port the network should listen on.
            // TODO: Other CVars
        }

        static CVar Register(string name, string value, CVarFlag flags)
        {
            return system.Register(name, value, flags);
        }
    }
}

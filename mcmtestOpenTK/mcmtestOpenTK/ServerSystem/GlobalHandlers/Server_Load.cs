﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using System.Threading;
using System.Diagnostics;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.Global;
using mcmtestOpenTK.ServerSystem.CommonHandlers;

namespace mcmtestOpenTK.ServerSystem.GlobalHandlers
{
    public partial class Server
    {
        /// <summary>
        /// Loads the server system.
        /// <returns>Whether loading worked.</returns>
        /// </summary>
        static bool ServerLoad()
        {
            ServerOutputter sout = new ServerOutputter();
            sout.Initializing = true;
            SysConsole.Output(OutputType.INIT, "Preparing CVar system...");
            ServerCVar.Init(sout);
            SysConsole.Output(OutputType.INIT, "Preparing command system...");
            ServerCommands.Init(sout);
            SysConsole.Output(OutputType.INIT, "Preparing console listener...");
            ConsoleHandler.Init();
            SysConsole.Output(OutputType.INIT, "Preparing global network system...");
            GlobalNetwork.Init();
            SysConsole.Output(OutputType.INIT, "Preparing network system...");
            if (!NetworkBase.Init())
            {
                return false;
            }
            sout.Initializing = false;
            return true;
        }
    }
}
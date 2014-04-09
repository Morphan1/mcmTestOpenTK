using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class ShowconsoleCommand: AbstractCommand
    {
        public ShowconsoleCommand()
        {
            Name = "showconsole";
            Arguments = "";
            Description = "Shows the internal system console for debugging purposes.";
            IsDebug = true;
        }

        public override void Execute(CommandInfo info)
        {
            SysConsole.ShowConsole();
            SysConsole.Output(OutputType.CLIENTINFO, "SysConsole showing [client command]");
            UIConsole.WriteLine(TextStyle.Color_Outgood + "Un-hid the system console.");
        }
    }
}

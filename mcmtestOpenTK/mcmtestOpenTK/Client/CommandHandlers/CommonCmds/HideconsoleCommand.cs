using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class HideconsoleCommand: AbstractCommand
    {
        public HideconsoleCommand()
        {
            Name = "hideconsole";
            Arguments = "";
            Description = "Hides the internal system console, for after debugging is done.";
            IsDebug = true;
        }

        public override void Execute(CommandEntry entry)
        {
            SysConsole.HideConsole();
            SysConsole.Output(OutputType.CLIENTINFO, "SysConsole hiding [client command]");
            UIConsole.WriteLine(TextStyle.Color_Outbad + "Hid the system console.");
        }
    }
}

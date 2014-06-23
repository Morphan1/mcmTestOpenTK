using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.CommonHandlers;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers
{
    public class ServerOutputter: Outputter
    {
        public override void WriteLine(string text)
        {
            SysConsole.Output(OutputType.INFO, text);
        }
        public override void Write(string text)
        {
            SysConsole.Write(text);
        }

        public override void Good(string tagged_text, DebugMode mode)
        {
            string text = ServerCommands.CommandSystem.TagSystem.ParseTags(tagged_text, TextStyle.Color_Outgood, null, mode);
            SysConsole.Output(OutputType.INFO, TextStyle.Color_Outgood + text);
        }

        public override void Bad(string tagged_text, DebugMode mode)
        {
            string text = ServerCommands.CommandSystem.TagSystem.ParseTags(tagged_text, TextStyle.Color_Outbad, null, mode);
            SysConsole.Output(OutputType.INFO, TextStyle.Color_Outbad + text);
        }

        public override void UnknownCommand(string basecommand, string[] arguments)
        {
            WriteLine(TextStyle.Color_Error + "Unknown command '" +
                TextStyle.Color_Standout + basecommand + TextStyle.Color_Error + "'.");
        }
    }
}

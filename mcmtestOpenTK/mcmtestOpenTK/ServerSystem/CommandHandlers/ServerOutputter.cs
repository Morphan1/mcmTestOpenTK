using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared;

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

        public override void Good(string tagged_text)
        {
            string text = ServerCommands.CommandSystem.TagSystem.ParseTags(tagged_text, TextStyle.Color_Outgood, null);
            SysConsole.Output(OutputType.INFO, TextStyle.Color_Outgood + text);
        }

        public override void Bad(string tagged_text)
        {
            string text = ServerCommands.CommandSystem.TagSystem.ParseTags(tagged_text, TextStyle.Color_Outbad, null);
            SysConsole.Output(OutputType.INFO, TextStyle.Color_Outbad + text);
        }
    }
}

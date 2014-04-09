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
            SysConsole.WriteLine(text);
        }
        public override void Write(string text)
        {
            SysConsole.Write(text);
        }
    }
}
